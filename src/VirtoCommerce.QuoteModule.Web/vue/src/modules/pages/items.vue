<template>
  <VcBlade
    :title="$t('QUOTES.PAGES.ITEMS.TITLE', { number: quote.item.value.number ?? '' })"
    :toolbar-items="bladeToolbar"
    :loading="adding"
    width="70%"
  >
    <VcContainer>
      <div class="quotes-form tw-space-y-4">
        <div
          v-if="!lines.length"
          class="quotes-empty"
        >
          {{ $t("QUOTES.PAGES.ITEMS.EMPTY") }}
        </div>
        <VcCard
          v-for="(line, index) in lines"
          :key="line.id ?? `new-${index}`"
          :header="line.name"
        >
          <template #actions>
            <VcButton
              icon="lucide-info"
              variant="ghost"
              size="sm"
              @click="openLine(index)"
              >{{ $t("QUOTES.PAGES.ITEMS.DETAILS") }}</VcButton
            >
            <VcCheckbox
              v-if="!locked"
              :model-value="selected.includes(line)"
              :aria-label="$t('QUOTES.PAGES.ITEMS.SELECT_ITEM')"
              @update:model-value="(v: unknown) => toggle(line, !!v)"
            />
          </template>
          <div class="quote-line">
            <button
              type="button"
              class="quote-line__image"
              :aria-label="line.name"
              @click="openProduct(line.productId)"
            >
              <VcImage
                :src="assetUrl(line.imageUrl)"
                size="s"
                aspect="1x1"
                bordered
                background="contain"
              />
            </button>
            <div class="quote-line__body">
              <div class="quote-line__facts">
                <span
                  ><em>{{ $t("QUOTES.PAGES.ITEMS.SKU") }}</em> {{ line.sku }}</span
                >
                <span
                  ><em>{{ $t("QUOTES.PAGES.ITEMS.LIST_PRICE") }}</em> {{ money(line.listPrice, currency) }}</span
                >
                <span
                  ><em>{{ $t("QUOTES.PAGES.ITEMS.SALE_PRICE") }}</em> {{ money(line.salePrice, currency) }}</span
                >
              </div>
              <VcTextarea
                v-model="line.comment"
                :label="$t('QUOTES.PAGES.ITEMS.COMMENT')"
                :placeholder="$t('QUOTES.PAGES.ITEMS.COMMENT_PLACEHOLDER')"
                :disabled="locked"
              />
              <table class="quote-tiers">
                <thead>
                  <tr>
                    <th />
                    <th>{{ $t("QUOTES.PAGES.ITEMS.QUANTITY") }}</th>
                    <th>{{ $t("QUOTES.PAGES.ITEMS.PRICE") }}</th>
                    <th class="quote-tiers__num">{{ $t("QUOTES.PAGES.ITEMS.MARGIN") }}</th>
                  </tr>
                </thead>
                <tbody>
                  <tr
                    v-for="(tier, t) in line.proposalPrices"
                    :key="t"
                  >
                    <td>
                      <VcRadioButton
                        :value="t"
                        :model-value="line.proposalPrices!.indexOf(line.selectedTierPrice!)"
                        :aria-label="$t('QUOTES.PAGES.ITEMS.SELECTED_TIER')"
                        :disabled="locked"
                        @update:model-value="line.selectedTierPrice = line.proposalPrices![t]"
                      />
                    </td>
                    <td>
                      <VcInput
                        v-model="tier.quantity"
                        type="integer"
                        size="small"
                        required
                        :aria-label="$t('QUOTES.PAGES.ITEMS.QUANTITY')"
                        :disabled="locked"
                      />
                    </td>
                    <td>
                      <VcInputCurrency
                        v-model="tier.price"
                        :option="currency"
                        size="small"
                        required
                        :aria-label="$t('QUOTES.PAGES.ITEMS.PRICE')"
                        :disabled="locked"
                      />
                    </td>
                    <td class="quote-tiers__num">{{ margin(line, tier) }}</td>
                  </tr>
                </tbody>
              </table>
              <div
                v-if="!locked"
                class="quote-line__tier-actions"
              >
                <VcButton
                  icon="lucide-plus"
                  variant="ghost"
                  size="sm"
                  @click="addTier(line)"
                  >{{ $t("QUOTES.PAGES.ITEMS.ADD_TIER") }}</VcButton
                >
                <VcButton
                  icon="lucide-minus"
                  variant="ghost"
                  size="sm"
                  :disabled="(line.proposalPrices?.length ?? 0) < 2"
                  @click="removeTier(line)"
                  >{{ $t("QUOTES.PAGES.ITEMS.REMOVE_TIER") }}</VcButton
                >
              </div>
            </div>
          </div>
        </VcCard>

        <div class="quote-summary">
          <VcCard :header="$t('QUOTES.PAGES.ITEMS.DISCOUNT_TITLE')">
              <div class="tw-p-4 tw-space-y-4">
                <div class="quote-discount">
                  <VcInputCurrency
                    v-if="absoluteDiscount"
                    v-model="q.manualSubTotal"
                    :label="`${$t('QUOTES.PAGES.ITEMS.NEW_SUBTOTAL')}, ${currency}`"
                    :option="currency"
                    :disabled="locked"
                  />
                  <VcInput
                    v-else
                    v-model="q.manualRelDiscountAmount"
                    type="number"
                    :label="$t('QUOTES.PAGES.ITEMS.RELATIVE_DISCOUNT')"
                    :disabled="locked"
                  />
                  <VcButton
                    variant="secondary"
                    size="sm"
                    :disabled="locked"
                    @click="toggleDiscountMode"
                    >{{
                      $t(absoluteDiscount ? "QUOTES.PAGES.ITEMS.ENTER_RELATIVE" : "QUOTES.PAGES.ITEMS.ENTER_SUBTOTAL")
                    }}</VcButton
                  >
                </div>
                <VcSelect
                  v-model="shippingMethodKey"
                  :label="$t('QUOTES.PAGES.ITEMS.SHIPPING_METHOD')"
                  :placeholder="$t('QUOTES.PAGES.ITEMS.SHIPPING_METHOD_PLACEHOLDER')"
                  :options="shippingOptions"
                  option-value="id"
                  option-label="title"
                  clearable
                  :disabled="locked"
                />
                <VcInputCurrency
                  v-model="q.manualShippingTotal"
                  :label="`${$t('QUOTES.PAGES.ITEMS.MANUAL_SHIPPING')}, ${currency}`"
                  :option="currency"
                  :disabled="locked"
                />
              </div>
          </VcCard>
          <VcCard :header="$t('QUOTES.PAGES.ITEMS.TOTALS.TITLE')">
              <TotalsList
                :rows="totals"
                :currency="currency"
              />
          </VcCard>
        </div>
      </div>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
// quote-items: the quote's line items with proposal price tiers, discount, shipping and totals.
// Edits stay in the quote opened by QuoteDetails until it is saved there.
import { useAsync, useBlade, useFunctions, type IBladeToolbar } from "@vc-shell/framework";
import {
  VcBlade,
  VcButton,
  VcCard,
  VcCheckbox,
  VcContainer,
  VcImage,
  VcInput,
  VcInputCurrency,
  VcRadioButton,
  VcSelect,
  VcTextarea,
} from "@vc-shell/framework/ui";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import type { QuoteItem, ShipmentMethod, TierPrice } from "../../api_client/virtocommerce.quote";
import { getProduct, getProductPrices, type ProductEntry } from "../api";
import TotalsList from "../components/TotalsList.vue";
import { useQuote } from "../composables/useQuote";
import { assetUrl, useMoney, useOpenProduct } from "../utils";

defineBlade({ name: "QuoteItems" });

const { t } = useI18n({ useScope: "global" });
const { options, openBlade, exposeToChildren } = useBlade<{ quoteId: string }>();
const { debounce } = useFunctions();
const money = useMoney();
const openProduct = useOpenProduct();
const quote = useQuote(options.value?.quoteId);
const q = quote.item;
const locked = quote.locked;

const currency = computed(() => q.value.currency);
const lines = computed(() => q.value.items ?? []);
const selected = ref<QuoteItem[]>([]);

const toggle = (line: QuoteItem, on: boolean) =>
  (selected.value = on ? [...selected.value, line] : selected.value.filter((x) => x !== line));

// margin of a proposed price against the sale (or list) price, as the legacy grid shows it
function margin(line: QuoteItem, tier: TierPrice) {
  const base = line.salePrice || line.listPrice;
  return tier.price && base ? `${Math.round(((tier.price - base) / tier.price) * 100)} %` : "";
}

function addTier(line: QuoteItem) {
  (line.proposalPrices ??= []).push({ quantity: 1, price: undefined });
}

// removes the selected tier (never the last one) and selects the first remaining
function removeTier(line: QuoteItem) {
  const tiers = line.proposalPrices ?? [];
  const i = tiers.indexOf(line.selectedTierPrice!);
  if (tiers.length < 2 || i < 0) return;
  tiers.splice(i, 1);
  line.selectedTierPrice = tiers[0];
}

// discount: either a new subtotal (absolute) or a relative discount in percent
const absoluteDiscount = ref((q.value.manualSubTotal ?? 0) > 0);
function toggleDiscountMode() {
  q.value.manualSubTotal = 0;
  q.value.manualRelDiscountAmount = 0;
  absoluteDiscount.value = !absoluteDiscount.value;
}

const methodKey = (m?: ShipmentMethod) => (m ? [m.shipmentMethodCode, m.optionName ?? "", m.currency].join("|") : undefined);
const shippingOptions = computed(() =>
  quote.shippingMethods.value
    .filter((m) => !m.currency || m.currency === currency.value)
    .map((m) => ({
      id: methodKey(m),
      title: `${m.typeName ?? m.shipmentMethodCode}${m.optionName ? ` (${m.optionName})` : ""} - ${money(m.price, m.currency)}`,
    })),
);
const shippingMethodKey = computed({
  get: () => methodKey(q.value.shipmentMethod),
  set: (key) => (q.value.shipmentMethod = quote.shippingMethods.value.find((m) => methodKey(m) === key)),
});

const totals = computed(() => {
  const x = q.value.totals ?? {};
  return [
    { label: t("QUOTES.PAGES.ITEMS.TOTALS.ORIGINAL_SUBTOTAL"), value: x.originalSubTotalExlTax },
    { label: t("QUOTES.PAGES.ITEMS.TOTALS.ADJUSTMENT"), value: x.adjustmentQuoteExlTax },
    { label: t("QUOTES.PAGES.ITEMS.TOTALS.SUBTOTAL"), value: x.subTotalExlTax },
    { label: t("QUOTES.PAGES.ITEMS.TOTALS.SHIPPING"), value: x.shippingTotal },
    { label: t("QUOTES.PAGES.ITEMS.TOTALS.DISCOUNT"), value: x.discountTotal },
    { label: t("QUOTES.PAGES.ITEMS.TOTALS.GRAND_TOTAL"), value: x.grandTotalExlTax, grand: true },
    { label: t("QUOTES.PAGES.ITEMS.TOTALS.TAXES"), value: x.taxTotal },
    { label: t("QUOTES.PAGES.ITEMS.TOTALS.GRAND_TOTAL_INCL_TAX"), value: x.grandTotalInclTax, grand: true },
  ];
});

// the legacy blade recalculates on a change of the lines, their tiers, the selected tier, discount or shipping
// (its ng-change handlers), not while a line comment is typed
watch(
  () =>
    JSON.stringify([
      lines.value.map((i) => [i.productId, i.proposalPrices?.map((p) => [p.quantity, p.price]), i.proposalPrices?.indexOf(i.selectedTierPrice!)]),
      q.value.manualSubTotal,
      q.value.manualRelDiscountAmount,
      q.value.manualShippingTotal,
      methodKey(q.value.shipmentMethod),
    ]),
  debounce(() => quote.recalculate(), 400),
);

// products picked in QuoteProductPicker become new lines with the catalog prices in the quote's currency
const { loading: adding, action: addProducts } = useAsync<ProductEntry[]>(async (products) => {
  // fetched in parallel, added in order: the lines before a failed product are kept
  const loaded = await Promise.allSettled(
    (products ?? []).map((entry) => Promise.all([getProduct(entry.id!), getProductPrices(entry.id!).catch(() => [])])),
  );
  for (const result of loaded) {
    if (result.status === "rejected") throw result.reason;
    const [product, prices] = result.value;
    const price = prices.find((p) => p.currency === currency.value);
    const tier: TierPrice = { quantity: 1, price: undefined };
    (q.value.items ??= []).push({
      productId: product.id,
      sku: product.code,
      catalogId: product.catalogId,
      categoryId: product.categoryId,
      name: product.name,
      imageUrl: product.imgSrc,
      taxType: product.taxType,
      quantity: 1,
      listPrice: price?.list ?? 0,
      salePrice: price?.sale ?? 0,
      currency: currency.value,
      proposalPrices: [tier],
      selectedTierPrice: tier,
    });
  }
});

const openLine = (index: number) =>
  openBlade({ name: "QuoteItemDetails", options: { quoteId: options.value?.quoteId, index } });

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "add",
    title: t("QUOTES.TOOLBAR.ADD_ITEM"),
    icon: "lucide-plus",
    permissions: "quote:update",
    disabled: locked,
    clickHandler: () => openBlade({ name: "QuoteProductPicker" }),
  },
  {
    id: "remove",
    title: t("QUOTES.TOOLBAR.REMOVE"),
    icon: "lucide-trash-2",
    permissions: "quote:update",
    disabled: computed(() => locked.value || !selected.value.length),
    clickHandler: () => {
      q.value.items = lines.value.filter((x) => !selected.value.includes(x));
      selected.value = [];
    },
  },
]);

exposeToChildren({ addProducts: (products: ProductEntry[]) => addProducts(products) });
</script>

<style lang="scss">
.quote-line {
  display: flex;
  gap: 16px;
  padding: 16px;
  &__image {
    flex: none;
    padding: 0;
    border: 0;
    background: none;
    cursor: pointer;
    align-self: flex-start;
  }
  &__body {
    flex: 1;
    min-width: 0;
    display: flex;
    flex-direction: column;
    gap: 12px;
  }
  &__facts {
    display: flex;
    flex-wrap: wrap;
    gap: 4px 20px;
    font-size: 13px;
    font-variant-numeric: tabular-nums;
    color: var(--color-neutral-900);
    span {
      white-space: nowrap;
    }
    em {
      font-style: normal;
      color: var(--color-neutral-500);
      margin-right: 4px;
    }
  }
  &__tier-actions {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
  }
}

.quote-tiers {
  width: 100%;
  border-collapse: collapse;
  font-size: 13px;
  th {
    text-align: left;
    font-weight: 500;
    color: var(--color-neutral-500);
    padding: 0 8px 4px 0;
  }
  td {
    padding: 4px 8px 4px 0;
    vertical-align: middle;
  }
  th:first-child,
  td:first-child {
    width: 32px;
  }
  /* quantity stays compact so the price input gets the room */
  th:nth-child(2),
  td:nth-child(2) {
    width: 72px;
  }
  &__num {
    width: 1%;
    text-align: right !important;
    white-space: nowrap;
    font-variant-numeric: tabular-nums;
    color: var(--color-neutral-700);
  }
}

.quote-discount {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  gap: 8px;
  > :first-child {
    align-self: stretch;
  }
}

/* phones: the image goes above the line so the tier inputs get the full width */
@media (max-width: 640px) {
  .quote-line {
    flex-direction: column;
  }
}

/* side by side when the blade is wide, stacked when narrow */
.quote-summary {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 16px;
  align-items: start;
}
</style>
