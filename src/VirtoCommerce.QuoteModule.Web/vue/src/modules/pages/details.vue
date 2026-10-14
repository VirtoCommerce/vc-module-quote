<template>
  <VcBlade
    :title="title"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="70%"
  >
    <VcContainer>
      <VcForm class="quotes-form tw-space-y-4">
        <div
          v-if="item.status"
          class="quotes-state"
        >
          <VcStatus :variant="statusVariant(item.status)">{{ item.status }}</VcStatus>
          <VcStatus
            v-if="item.isLocked"
            variant="warning"
            >{{ $t("QUOTES.PAGES.DETAILS.ON_HOLD") }}</VcStatus
          >
          <VcHint v-if="item.isCancelled && item.cancelReason">{{ item.cancelReason }}</VcHint>
        </div>
        <VcRow class="tw-gap-4">
          <VcCol>
            <div class="quotes-link-field">
              <VcLabel>{{ $t("QUOTES.PAGES.DETAILS.FIELDS.CUSTOMER") }}</VcLabel>
              <VcLink
                v-if="item.customerName"
                @click="openCustomer"
                >{{ item.customerName }}</VcLink
              >
              <span v-else>—</span>
            </div>
          </VcCol>
          <VcCol>
            <VcField
              :label="$t('QUOTES.PAGES.DETAILS.FIELDS.CREATED')"
              :model-value="item.createdDate"
              type="date"
            />
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <div class="quotes-link-field">
              <VcLabel>{{ $t("QUOTES.PAGES.DETAILS.FIELDS.ORGANIZATION") }}</VcLabel>
              <VcLink
                v-if="item.organizationId"
                @click="openOrganization"
                >{{ item.organizationName || item.organizationId }}</VcLink
              >
              <span v-else>{{ $t("QUOTES.PAGES.DETAILS.FIELDS.NOT_DEFINED") }}</span>
            </div>
          </VcCol>
          <VcCol>
            <VcField
              :label="$t('QUOTES.PAGES.DETAILS.FIELDS.MODIFIED')"
              :model-value="item.modifiedDate"
              type="date"
            />
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <Field
              v-slot="{ errorMessage, handleChange, errors }"
              name="status"
              rules="required"
              :model-value="item.status"
            >
              <VcSelect
                v-model="item.status"
                :label="$t('QUOTES.PAGES.DETAILS.FIELDS.STATUS')"
                :options="statusOptions"
                option-value="id"
                option-label="title"
                required
                :clearable="false"
                :disabled="locked"
                :error="!!errors.length"
                :error-message="errorMessage"
                @update:model-value="handleChange"
              >
                <template
                  v-if="hasAccess('platform:setting:update')"
                  #append
                >
                  <VcButton
                    icon="lucide-pencil"
                    icon-size="s"
                    text
                    :aria-label="$t('QUOTES.TOOLBAR.EDIT_STATUSES')"
                    @click="openStatuses"
                  />
                </template>
              </VcSelect>
            </Field>
          </VcCol>
          <VcCol>
            <Field
              v-slot="{ errorMessage, handleChange, errors }"
              name="storeId"
              rules="required"
              :model-value="item.storeId"
            >
              <VcSelect
                v-model="item.storeId"
                :label="$t('QUOTES.PAGES.DETAILS.FIELDS.STORE')"
                :placeholder="$t('QUOTES.PAGES.DETAILS.FIELDS.STORE_PLACEHOLDER')"
                :options="searchStores"
                option-value="id"
                option-label="name"
                searchable
                required
                :clearable="false"
                :disabled="locked"
                :error="!!errors.length"
                :error-message="errorMessage"
                @update:model-value="handleChange"
              />
            </Field>
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcDatePicker
              v-model="item.expirationDate"
              :label="$t('QUOTES.PAGES.DETAILS.FIELDS.EXPIRATION')"
              :disabled="locked"
              :date-picker-options="{ minDate: today }"
            />
          </VcCol>
          <VcCol>
            <VcDatePicker
              v-model="item.reminderDate"
              :label="$t('QUOTES.PAGES.DETAILS.FIELDS.REMINDER')"
              :disabled="locked"
              :date-picker-options="{ minDate: today }"
            />
          </VcCol>
        </VcRow>
        <VcRow class="tw-gap-4">
          <VcCol>
            <VcSelect
              v-model="item.employeeId"
              :label="$t('QUOTES.PAGES.DETAILS.FIELDS.EMPLOYEE')"
              :placeholder="$t('QUOTES.PAGES.DETAILS.FIELDS.EMPLOYEE_PLACEHOLDER')"
              :options="searchEmployees"
              option-value="id"
              option-label="name"
              searchable
              :disabled="locked"
              @update:model-value="onEmployeeChanged"
            />
          </VcCol>
          <VcCol />
        </VcRow>
        <VcTextarea
          v-model="item.comment"
          :label="$t('QUOTES.PAGES.DETAILS.FIELDS.COMMENT')"
          :placeholder="$t('QUOTES.PAGES.DETAILS.FIELDS.COMMENT_PLACEHOLDER')"
          :disabled="locked"
        />
        <VcTextarea
          v-model="item.innerComment"
          :label="$t('QUOTES.PAGES.DETAILS.FIELDS.INNER_COMMENT')"
          :placeholder="$t('QUOTES.PAGES.DETAILS.FIELDS.INNER_COMMENT')"
          :disabled="locked"
        />

        <!-- the legacy totals widget: opens the line items blade -->
        <VcCard :header="$t('QUOTES.PAGES.DETAILS.TOTALS.TITLE')">
          <template #actions>
            <VcButton
              icon="lucide-list"
              variant="ghost"
              size="sm"
              @click="openItems"
              >{{ $t("QUOTES.PAGES.DETAILS.TOTALS.EDIT_ITEMS") }}</VcButton
            >
          </template>
          <TotalsList
            :rows="totals"
            :currency="item.currency"
          />
        </VcCard>
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
import {
  notification,
  useApiClient,
  useAsync,
  useBlade,
  useBladeForm,
  useBladeWidgets,
  useLoading,
  usePermissions,
  usePopup,
  type IBladeToolbar,
} from "@vc-shell/framework";
import {
  VcBlade,
  VcButton,
  VcCard,
  VcCol,
  VcContainer,
  VcDatePicker,
  VcField,
  VcForm,
  VcHint,
  VcLabel,
  VcLink,
  VcRow,
  VcSelect,
  VcStatus,
  VcTextarea,
} from "@vc-shell/framework/ui";
import { Field } from "vee-validate";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import { QuoteModuleClient, type QuoteRequest, type ShipmentMethod } from "../../api_client/virtocommerce.quote";
import { getMemberByUserId, searchEmployees, searchStores, settingValues } from "../api";
import TotalsList from "../components/TotalsList.vue";
import { linkSelectedTiers, provideQuote } from "../composables/useQuote";
import { useCancelPrompt } from "../composables/useCancelPrompt";
import { useDynamicPropertyCount } from "../composables/useDynamicProps";
import { statusVariant } from "../utils";

defineBlade({
  url: "/quote",
  name: "QuoteDetails",
  permissions: ["quote:read"],
});

const PROPOSAL_SENT = "Proposal sent";
const CANCELLED = "Cancelled";

const { t } = useI18n({ useScope: "global" });
const { param, openBlade, callParent, closeSelf } = useBlade();
const { getApiClient } = useApiClient(QuoteModuleClient);
const { showConfirmation } = usePopup();
const { hasAccess } = usePermissions();
const askCancelReason = useCancelPrompt();

const item = ref<QuoteRequest>({});
const savedStatus = ref<string>();
const shippingMethods = ref<ShipmentMethod[]>([]);
const statuses = ref<string[]>([]);
const today = new Date(new Date().setHours(0, 0, 0, 0));

const form = useBladeForm({
  data: item,
  closeConfirmMessage: computed(() => t("QUOTES.ALERTS.CLOSE_CONFIRMATION")),
});

const locked = computed(() => !!item.value.isLocked);
const title = computed(() => item.value.number ?? t("QUOTES.PAGES.DETAILS.TITLE"));
// a status outside the dictionary (renamed / removed value) stays selectable
const statusOptions = computed(() =>
  [...new Set([...statuses.value, ...(item.value.status ? [item.value.status] : [])])].map((x) => ({ id: x, title: x })),
);

const totals = computed(() => {
  const x = item.value.totals ?? {};
  return [
    { label: t("QUOTES.PAGES.DETAILS.TOTALS.SUBTOTAL"), value: x.subTotalExlTax },
    { label: t("QUOTES.PAGES.DETAILS.TOTALS.SHIPPING"), value: x.shippingTotal },
    { label: t("QUOTES.PAGES.DETAILS.TOTALS.DISCOUNT"), value: x.discountTotal },
    { label: t("QUOTES.PAGES.DETAILS.TOTALS.GRAND_TOTAL"), value: x.grandTotalInclTax, grand: true },
  ];
});

const { loading: itemLoading, action: load } = useAsync(async () => {
  const quote = await (await getApiClient()).getById(param.value!);
  linkSelectedTiers(quote);
  item.value = quote;
  savedStatus.value = quote.status;
  form.setBaseline();
});

const { loading: saveLoading, action: save } = useAsync(async () => {
  await (await getApiClient()).update(item.value);
  await load();
  await callParent("reload").catch(() => undefined);
});

// totals for the edited (unsaved) quote; only the totals are taken so the line item objects stay the same
const { loading: recalcLoading, action: recalculateAction } = useAsync(async () => {
  const result = await (await getApiClient()).calculateTotals(item.value);
  item.value.totals = result.totals;
});
const recalculate = () => recalculateAction();

const { loading: deleteLoading, action: remove } = useAsync(async () => {
  await (await getApiClient()).delete([item.value.id!]);
});

const loading = useLoading(itemLoading, saveLoading, recalcLoading, deleteLoading);

function onEmployeeChanged(value: unknown) {
  // the select emits the id; keep the denormalized name in step, like the AngularJS selector
  if (!value) item.value.employeeName = undefined;
  else searchEmployees(undefined, 0, [String(value)]).then((r) => (item.value.employeeName = r.results?.[0]?.name));
}

const loadStatuses = async () => (statuses.value = await settingValues("Quotes.Status").catch(() => []));
const openStatuses = () => openBlade({ name: "PlatformSettingDictionary", param: "Quotes.Status", onClose: loadStatuses });

async function submitProposal() {
  const q = item.value;
  const incomplete = !q.items?.length || !q.totals?.grandTotalInclTax;
  if (!(await showConfirmation(t(incomplete ? "QUOTES.ALERTS.PROPOSAL_SUBMIT_WARNING" : "QUOTES.ALERTS.PROPOSAL_SUBMIT"))))
    return;
  q.status = PROPOSAL_SENT;
  await save();
}

async function toggleHold() {
  if (!(await showConfirmation(t(locked.value ? "QUOTES.ALERTS.HOLD_RELEASE" : "QUOTES.ALERTS.HOLD_PLACE")))) return;
  item.value.isLocked = !item.value.isLocked;
  await save();
}

async function cancelQuote() {
  const reason = await askCancelReason();
  if (reason === false) return;
  Object.assign(item.value, { cancelReason: reason, isCancelled: true, status: CANCELLED });
  await save();
}

const bladeToolbar = ref<IBladeToolbar[]>([
  {
    id: "save",
    title: t("QUOTES.TOOLBAR.SAVE"),
    icon: "lucide-save",
    permissions: "quote:update",
    disabled: computed(() => locked.value || !form.canSave.value),
    clickHandler: () => save(),
  },
  {
    id: "reset",
    title: t("QUOTES.TOOLBAR.RESET"),
    icon: "lucide-undo-2",
    permissions: "quote:update",
    disabled: computed(() => !form.isModified.value),
    clickHandler: () => load(),
  },
  {
    id: "submitProposal",
    title: t("QUOTES.TOOLBAR.SUBMIT_PROPOSAL"),
    icon: "lucide-square-check",
    permissions: "quote:update",
    disabled: computed(() => !item.value.id || savedStatus.value === PROPOSAL_SENT),
    clickHandler: submitProposal,
  },
  {
    id: "hold",
    title: computed(() => t(locked.value ? "QUOTES.TOOLBAR.RELEASE_HOLD" : "QUOTES.TOOLBAR.PLACE_ON_HOLD")),
    icon: () => (locked.value ? "lucide-lock-open" : "lucide-lock"),
    permissions: "quote:update",
    disabled: computed(() => !item.value.id),
    clickHandler: toggleHold,
  },
  {
    id: "cancel",
    title: t("QUOTES.TOOLBAR.CANCEL_DOCUMENT"),
    icon: "lucide-ban",
    permissions: "quote:update",
    disabled: computed(() => !item.value.id || !!item.value.isCancelled),
    clickHandler: cancelQuote,
  },
  {
    id: "delete",
    title: t("QUOTES.TOOLBAR.DELETE"),
    icon: "lucide-trash-2",
    permissions: "quote:delete",
    async clickHandler() {
      if (!(await showConfirmation(t("QUOTES.ALERTS.DELETE")))) return;
      await remove();
      form.setBaseline();
      await callParent("reload").catch(() => undefined);
      await closeSelf();
    },
  },
]);

async function openMember(memberId: string | undefined, memberType: string) {
  if (!memberId) {
    notification.error(t("QUOTES.ALERTS.MEMBER_NOT_FOUND"), { timeout: 5000 });
    return;
  }
  const name =
    memberType === "Organization"
      ? "CustomerOrganizationDetails"
      : memberType === "Employee"
        ? "CustomerEmployeeDetails"
        : "CustomerContactDetails";
  // owned by the Customer plugin; it may not be installed in this host
  await openBlade({ name, param: memberId }).catch(() =>
    notification.error(t("QUOTES.ALERTS.MEMBER_UNAVAILABLE"), { timeout: 5000 }),
  );
}

async function openCustomer() {
  if (!item.value.customerId) return;
  const member = await getMemberByUserId(item.value.customerId).catch(() => undefined);
  await openMember(member?.id, member?.memberType ?? "Contact");
}
const openOrganization = () => openMember(item.value.organizationId, "Organization");

// the key provideQuote registered the quote under below, whatever the param was (item.id may differ from it)
const childOptions = () => ({ quoteId: param.value });
const openItems = () => openBlade({ name: "QuoteItems", options: childOptions() });
const openAddresses = () => openBlade({ name: "QuoteAddresses", options: childOptions() });

const dynamicPropertyCount = useDynamicPropertyCount(computed(() => item.value.objectType));

useBladeWidgets([
  {
    id: "QuoteItems",
    icon: "lucide-list",
    title: "QUOTES.WIDGETS.ITEMS",
    badge: computed(() => item.value.items?.length ?? 0),
    onClick: openItems,
  },
  {
    id: "QuoteAddresses",
    icon: "lucide-map-pin",
    title: "QUOTES.WIDGETS.ADDRESSES",
    badge: computed(() => item.value.addresses?.length ?? 0),
    onClick: openAddresses,
  },
  {
    id: "QuoteAttachments",
    icon: "lucide-paperclip",
    title: "QUOTES.WIDGETS.ATTACHMENTS",
    badge: computed(() => item.value.attachments?.length ?? 0),
    onClick: () => openBlade({ name: "QuoteAttachments", options: childOptions() }),
  },
  {
    id: "QuoteDynamicProperties",
    icon: "lucide-list-plus",
    title: "QUOTES.WIDGETS.PROPERTIES",
    badge: dynamicPropertyCount,
    isVisible: hasAccess("platform:dynamic_properties:read"),
    onClick: () => openBlade({ name: "QuoteDynamicProperties", options: childOptions() }),
  },
  {
    id: "QuoteChanges",
    icon: "lucide-history",
    title: "QUOTES.WIDGETS.CHANGES",
    onClick: () => openBlade({ name: "QuoteChanges", param: item.value.id }),
  },
]);

provideQuote(param.value!, { item, locked, shippingMethods, recalculate });

onMounted(async () => {
  const client = await getApiClient();
  await Promise.all([
    load(),
    loadStatuses(),
    // 404 when the quote's store is gone: no methods to offer
    client.getShipmentMethods(param.value!).then(
      (methods) => (shippingMethods.value = methods),
      () => (shippingMethods.value = []),
    ),
  ]);
});
</script>

<style lang="scss">
/* the design's blade body padding (ProductDetail.jsx: 20px 22px 36px); shared by the quote's child blades */
.quotes-form {
  padding: 20px 22px 36px;
}

.quotes-state {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 12px;
}

.quotes-link-field {
  display: flex;
  flex-direction: column;
  gap: 6px;
  font-size: 14px;
}

.quotes-empty {
  padding: 24px 0;
  text-align: center;
  font-size: 14px;
  color: var(--color-neutral-500);
}
</style>
