<template>
  <VcBlade
    :title="$t('QUOTES.WIDGETS.ADDRESSES')"
    :toolbar-items="bladeToolbar"
    width="50%"
  >
    <VcDataTable
      :items="addresses"
      state-key="QUOTE_ADDRESSES"
      :active-item-id="activeItemId"
      :empty-state="{ icon: 'lucide-map-pin', title: $t('QUOTES.PAGES.ADDRESSES.EMPTY') }"
      @row-click="(e: { data: QuoteAddress }) => open(addresses.indexOf(e.data))"
    >
      <VcColumn
        id="name"
        :title="$t('QUOTES.PAGES.ADDRESSES.ADDRESS')"
        always-visible
        mobile-role="title"
      >
        <template #body="{ data: a }">{{ addressName(a) }}</template>
      </VcColumn>
      <VcColumn
        id="addressType"
        :title="$t('QUOTES.PAGES.ADDRESSES.TYPE')"
        mobile-role="field"
      />
      <VcColumn
        id="person"
        :title="$t('QUOTES.PAGES.ADDRESSES.PERSON')"
        mobile-role="field"
      >
        <template #body="{ data: a }">{{ [a.firstName, a.lastName].filter(Boolean).join(" ") }}</template>
      </VcColumn>
      <VcColumn
        id="isDefault"
        :title="$t('QUOTES.PAGES.ADDRESSES.DEFAULT')"
        mobile-role="status"
      >
        <template #body="{ data: a }">
          <div
            v-if="a.isDefault"
            class="tw-inline-flex"
          >
            <VcStatus variant="success">{{ $t("QUOTES.PAGES.ADDRESSES.DEFAULT") }}</VcStatus>
          </div>
        </template>
      </VcColumn>
    </VcDataTable>
  </VcBlade>
</template>

<script setup lang="ts">
// address-list: the quote's addresses; edits stay in the quote until it is saved
import { useBlade, type IBladeToolbar } from "@vc-shell/framework";
import { VcBlade, VcColumn, VcDataTable, VcStatus } from "@vc-shell/framework/ui";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import type { QuoteAddress } from "../../api_client/virtocommerce.quote";
import { useQuote } from "../composables/useQuote";
import { addressName } from "../utils";

defineBlade({ name: "QuoteAddresses" });

const { t } = useI18n({ useScope: "global" });
const { options, openBlade } = useBlade<{ quoteId: string }>();
const quote = useQuote(options.value?.quoteId);
const addresses = computed(() => quote.item.value.addresses ?? []);
const activeItemId = ref<string>();

const open = (index?: number) => {
  activeItemId.value = index === undefined ? undefined : addresses.value[index]?.id;
  openBlade({
    name: "QuoteAddressDetails",
    options: { quoteId: options.value?.quoteId, index },
    onClose: () => (activeItemId.value = undefined),
  });
};

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "add",
    title: t("QUOTES.TOOLBAR.ADD"),
    icon: "lucide-plus",
    permissions: "quote:update",
    disabled: quote.locked,
    clickHandler: () => open(),
  },
]);

// like the AngularJS list: nothing to pick from, so start a new address right away
onMounted(() => {
  if (!addresses.value.length && !quote.locked.value) open();
});
</script>
