<template>
  <VcBlade
    :title="isNew ? $t('QUOTES.PAGES.ADDRESSES.NEW_TITLE') : $t('QUOTES.PAGES.ADDRESSES.TITLE')"
    :toolbar-items="bladeToolbar"
    width="40%"
  >
    <VcContainer>
      <VcForm class="quotes-form tw-space-y-4">
        <VcTextarea
          v-model="address.description"
          :label="$t('QUOTES.PAGES.ADDRESSES.DESCRIPTION')"
          maxlength="128"
          :disabled="locked"
        />
        <VcSelect
          v-model="address.addressType"
          :label="$t('QUOTES.PAGES.ADDRESSES.TYPE')"
          :options="addressTypes"
          option-value="id"
          option-label="title"
          :clearable="false"
          :disabled="locked"
        />
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcInput
              v-model="address.firstName"
              :label="$t('QUOTES.PAGES.ADDRESSES.FIRST_NAME')"
              :disabled="locked"
            />
          </VcCol>
          <VcCol>
            <VcInput
              v-model="address.middleName"
              :label="$t('QUOTES.PAGES.ADDRESSES.MIDDLE_NAME')"
              :disabled="locked"
            />
          </VcCol>
          <VcCol>
            <VcInput
              v-model="address.lastName"
              :label="$t('QUOTES.PAGES.ADDRESSES.LAST_NAME')"
              :disabled="locked"
            />
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <Field
              v-slot="{ errorMessage, handleChange, errors }"
              name="countryCode"
              rules="required"
              :model-value="address.countryCode"
            >
              <VcSelect
                v-model="address.countryCode"
                :label="$t('QUOTES.PAGES.ADDRESSES.COUNTRY')"
                :options="countries"
                option-value="id"
                option-label="name"
                searchable
                required
                :disabled="locked"
                :error="!!errors.length"
                :error-message="errorMessage"
                @update:model-value="(v: unknown) => onCountryPicked(v, handleChange)"
              />
            </Field>
          </VcCol>
          <VcCol>
            <VcSelect
              v-model="address.regionName"
              :label="$t('QUOTES.PAGES.ADDRESSES.REGION')"
              :options="regions"
              option-value="name"
              option-label="name"
              searchable
              :disabled="locked"
              @update:model-value="onRegionPicked"
            />
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <Field
              v-slot="{ errorMessage, handleChange, errors }"
              name="city"
              rules="required"
              :model-value="address.city"
            >
              <VcInput
                v-model="address.city"
                :label="$t('QUOTES.PAGES.ADDRESSES.CITY')"
                required
                :disabled="locked"
                :error="!!errors.length"
                :error-message="errorMessage"
                @update:model-value="handleChange"
              />
            </Field>
          </VcCol>
          <VcCol>
            <Field
              v-slot="{ errorMessage, handleChange, errors }"
              name="postalCode"
              rules="required"
              :model-value="address.postalCode"
            >
              <VcInput
                v-model="address.postalCode"
                :label="$t('QUOTES.PAGES.ADDRESSES.POSTAL_CODE')"
                required
                :disabled="locked"
                :error="!!errors.length"
                :error-message="errorMessage"
                @update:model-value="handleChange"
              />
            </Field>
          </VcCol>
        </VcRow>
        <Field
          v-slot="{ errorMessage, handleChange, errors }"
          name="line1"
          rules="required"
          :model-value="address.line1"
        >
          <VcInput
            v-model="address.line1"
            :label="$t('QUOTES.PAGES.ADDRESSES.LINE1')"
            required
            :disabled="locked"
            :error="!!errors.length"
            :error-message="errorMessage"
            @update:model-value="handleChange"
          />
        </Field>
        <VcInput
          v-model="address.line2"
          :label="$t('QUOTES.PAGES.ADDRESSES.LINE2')"
          :disabled="locked"
        />
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcInput
              v-model="address.email"
              type="email"
              :label="$t('QUOTES.PAGES.ADDRESSES.EMAIL')"
              :disabled="locked"
            />
          </VcCol>
          <VcCol>
            <VcInput
              v-model="address.phone"
              :label="$t('QUOTES.PAGES.ADDRESSES.PHONE')"
              :disabled="locked"
            />
          </VcCol>
        </VcRow>
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
// address-details of a quote address: OK writes it into the quote (saved with the quote)
import { useBlade, useBladeForm, usePopup, type IBladeToolbar } from "@vc-shell/framework";
import { VcBlade, VcCol, VcContainer, VcForm, VcInput, VcRow, VcSelect, VcTextarea } from "@vc-shell/framework/ui";
import { Field } from "vee-validate";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import type { AddressType, QuoteAddress } from "../../api_client/virtocommerce.quote";
import { getCountries, getRegions } from "../api";
import { useQuote } from "../composables/useQuote";
import { addressName, clone } from "../utils";

defineBlade({ name: "QuoteAddressDetails" });

const { t } = useI18n({ useScope: "global" });
const { options, closeSelf } = useBlade<{ quoteId: string; index?: number }>();
const { showConfirmation } = usePopup();
const quote = useQuote(options.value?.quoteId);
const locked = quote.locked;

const addressTypes = ["Billing", "Shipping", "BillingAndShipping"].map((x) => ({ id: x, title: x }));
const current = () => (options.value?.index === undefined ? undefined : quote.item.value.addresses?.[options.value.index]);
const blank = (): QuoteAddress => ({ addressType: "Shipping" as AddressType });
const isNew = computed(() => !current());
const address = ref<QuoteAddress>(clone(current() ?? blank()));
const form = useBladeForm({ data: address });

const countries = ref<{ id?: string; name?: string }[]>([]);
const regions = ref<{ id?: string; name?: string }[]>([]);
getCountries().then((x) => (countries.value = x ?? []));

async function loadRegions(code?: string) {
  regions.value = code ? ((await getRegions(code).catch(() => [])) ?? []) : [];
}

function onCountryPicked(code: unknown, handleChange: (v: unknown) => void) {
  handleChange(code);
  address.value.countryName = countries.value.find((c) => c.id === code)?.name;
  address.value.regionName = undefined;
  address.value.regionId = undefined;
  loadRegions(code as string);
}

function onRegionPicked(name: unknown) {
  address.value.regionId = regions.value.find((r) => r.name === name)?.id;
}

onMounted(() => {
  form.setBaseline();
  loadRegions(address.value.countryCode);
});

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "apply",
    title: t("QUOTES.TOOLBAR.APPLY"),
    icon: "lucide-check",
    permissions: "quote:update",
    disabled: computed(() => locked.value || !form.canSave.value),
    async clickHandler() {
      const value = { ...clone(address.value), name: addressName(address.value) };
      const list = (quote.item.value.addresses ??= []);
      if (isNew.value) list.push(value);
      else list.splice(options.value!.index!, 1, value);
      form.setBaseline();
      await closeSelf();
    },
  },
  {
    id: "reset",
    title: t("QUOTES.TOOLBAR.RESET"),
    icon: "lucide-undo-2",
    disabled: computed(() => !form.isModified.value),
    clickHandler: () => {
      address.value = clone(current() ?? blank());
      form.setBaseline();
    },
  },
  {
    id: "delete",
    title: t("QUOTES.TOOLBAR.DELETE"),
    icon: "lucide-trash-2",
    permissions: "quote:update",
    isVisible: computed(() => !isNew.value),
    disabled: locked,
    async clickHandler() {
      if (!(await showConfirmation(t("QUOTES.ALERTS.DELETE_ADDRESS")))) return;
      const list = quote.item.value.addresses ?? [];
      const [removed] = list.splice(options.value!.index!, 1);
      // legacy: the last remaining address of that type becomes the default one
      const sameType = list.filter((a) => a.addressType === removed?.addressType);
      if (sameType.length === 1 && removed?.addressType !== "BillingAndShipping") sameType[0].isDefault = true;
      form.setBaseline();
      await closeSelf();
    },
  },
]);
</script>
