<template>
  <VcBlade
    :title="$t('QUOTES.PAGES.LIST.TITLE')"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="50%"
  >
    <VcDataTable
      v-model:search-value="searchValue"
      v-model:sort-field="sortField"
      v-model:sort-order="sortOrder"
      v-model:selection="selection"
      selection-mode="multiple"
      :items="items"
      :total-count="totalCount"
      :pagination="pagination"
      :searchable="true"
      search-bar
      state-key="QUOTES"
      :global-filters="globalFilters"
      :search-views="searchViews"
      :active-item-id="activeChildParam"
      :show-all-columns="!activeChildParam"
      :row-actions="rowActions"
      row-actions-mode="dropdown"
      row-actions-position="column"
      :empty-state="{ icon: 'lucide-file-question', title: $t('QUOTES.PAGES.LIST.EMPTY') }"
      @row-click="(e: { data: QuoteRequest }) => openDetails(e.data)"
      @pagination-click="pagination.goToPage"
      @filter="onFilter"
    >
      <VcColumn
        id="number"
        :title="$t('QUOTES.PAGES.LIST.COLUMNS.NUMBER')"
        body-class="quotes-list__number"
        sortable
        always-visible
        mobile-role="title"
      />
      <VcColumn
        id="status"
        :title="$t('QUOTES.PAGES.LIST.COLUMNS.STATUS')"
        sortable
        always-visible
        mobile-role="status"
      >
        <template #body="{ data: quote }">
          <div
            v-if="quote.status"
            class="tw-inline-flex"
          >
            <VcStatus :variant="statusVariant(quote.status)">{{ quote.status }}</VcStatus>
          </div>
        </template>
      </VcColumn>
      <VcColumn
        id="itemsCount"
        :title="$t('QUOTES.PAGES.LIST.COLUMNS.ITEMS')"
        type="number"
        align="end"
        width="110px"
        mobile-role="field"
      >
        <template #body="{ data: quote }">
          <span class="vc-table-cell-number">{{ quote.items?.length ?? 0 }}</span>
        </template>
      </VcColumn>
      <VcColumn
        id="customerName"
        :title="$t('QUOTES.PAGES.LIST.COLUMNS.CUSTOMER')"
        sortable
        mobile-role="field"
      />
      <VcColumn
        id="createdDate"
        :title="$t('QUOTES.PAGES.LIST.COLUMNS.CREATED')"
        type="date-ago"
        sortable
        mobile-role="field"
      />
    </VcDataTable>
  </VcBlade>
</template>

<script setup lang="ts">
import {
  useApiClient,
  useAsync,
  useBlade,
  useDataTablePagination,
  useDataTableSort,
  useFunctions,
  useLoading,
  usePermissions,
  usePopup,
  useTableSearch,
  type GlobalFilterConfig,
  type IBladeToolbar,
  type SearchBarView,
} from "@vc-shell/framework";
import { VcBlade, VcColumn, VcDataTable, VcStatus } from "@vc-shell/framework/ui";
import { computed, onMounted, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { QuoteModuleClient, type QuoteRequest } from "../../api_client/virtocommerce.quote";
import { searchStores, settingValues } from "../api";
import { statusVariant } from "../utils";

defineBlade({
  url: "/quotes",
  name: "QuotesList",
  isWorkspace: true,
  permissions: ["quote:access"],
  menuItem: {
    title: "QUOTES.MENU.TITLE",
    icon: "lucide-file-question",
    priority: 35,
  },
});

const PAGE_SIZE = 20;
const { t } = useI18n({ useScope: "global" });
const { activeChildParam, openBlade, closeChildren, exposeToChildren } = useBlade();
const { getApiClient } = useApiClient(QuoteModuleClient);
const { debounce } = useFunctions();
const { showConfirmation } = usePopup();
const { hasAccess } = usePermissions();

const items = ref<QuoteRequest[]>([]);
const totalCount = ref(0);
const selection = ref<QuoteRequest[]>([]);

const { sortField, sortOrder, sortExpression } = useDataTableSort({
  stateKey: "QUOTES",
  initialField: "createdDate",
  initialDirection: "DESC",
});
const { searchValue } = useTableSearch({ stateKey: "QUOTES" });
const pagination = useDataTablePagination({ stateKey: "QUOTES", pageSize: PAGE_SIZE, totalCount });

// A sales rep works the quotes by status (the menu counter's "waiting for a proposal" is Processing),
// by store in a multi-store setup, and by when the request came in.
const statuses = ref<string[]>([]);
const stores = ref<{ id?: string; name?: string }[]>([]);
const globalFilters = computed<GlobalFilterConfig[]>(() => [
  {
    id: "statuses",
    label: t("QUOTES.PAGES.LIST.COLUMNS.STATUS"),
    filter: { options: statuses.value.map((x) => ({ value: x, label: x })), multiple: true },
  },
  {
    id: "storeId",
    label: t("QUOTES.PAGES.LIST.FILTERS.STORE"),
    filter: { options: stores.value.map((x) => ({ value: x.id!, label: x.name ?? x.id! })) },
  },
  {
    id: "createdDate",
    label: t("QUOTES.PAGES.LIST.COLUMNS.CREATED"),
    filter: { range: ["startDate", "endDate"] },
  },
]);
const searchViews = computed<SearchBarView[]>(() => [
  { id: "waiting", name: t("QUOTES.PAGES.LIST.VIEWS.WAITING"), values: { statuses: ["Processing"] } },
]);

const filters = ref<{ statuses?: string[]; storeId?: string; startDate?: Date; endDate?: Date }>({});
// the search bar sends days (YYYY-MM-DD, both ends included); the API compares times, so the end is that day's last moment
const localDay = (ymd: unknown, endOfDay = false) => {
  if (typeof ymd !== "string" || !ymd) return undefined;
  const day = new Date(`${ymd}T00:00`);
  return endOfDay ? new Date(day.getFullYear(), day.getMonth(), day.getDate() + 1, 0, 0, 0, -1) : day;
};

function onFilter(event: { filters: Record<string, unknown> }) {
  const picked = ([] as string[]).concat((event.filters.statuses as string | string[] | undefined) ?? []);
  filters.value = {
    statuses: picked.length ? picked : undefined,
    storeId: (event.filters.storeId as string) || undefined,
    startDate: localDay(event.filters.startDate),
    endDate: localDay(event.filters.endDate, true),
  };
  // from page 2 setPage(1) triggers the skip watcher, which loads
  if (pagination.skip) pagination.setPage(1);
  else load();
}

async function loadFilterOptions() {
  const [statusValues, storeResult] = await Promise.all([
    settingValues("Quotes.Status").catch(() => [] as string[]),
    searchStores(undefined, 0, undefined, 100).catch(() => ({ results: [] })),
  ]);
  statuses.value = statusValues;
  stores.value = storeResult.results ?? [];
}

const { loading: searchLoading, action: load } = useAsync(async () => {
  const result = await (
    await getApiClient()
  ).search({
    keyword: searchValue.value || undefined,
    ...filters.value,
    sort: sortExpression.value,
    skip: pagination.skip,
    take: PAGE_SIZE,
  });
  items.value = result.results ?? [];
  totalCount.value = result.totalCount ?? 0;
});

const { loading: deleteLoading, action: remove } = useAsync<string[]>(async (ids) => {
  await (await getApiClient()).delete(ids ?? []);
});
const loading = useLoading(searchLoading, deleteLoading);

onMounted(() => {
  load();
  // the filters' options; without them the filters just have none to offer
  loadFilterOptions();
});
watch(searchValue, () => pagination.setPage(1));
watch([sortExpression, searchValue, () => pagination.skip], debounce(load, 300));

async function deleteQuotes(quotes: QuoteRequest[]) {
  if (!(await showConfirmation(t("QUOTES.ALERTS.DELETE_SELECTED")))) return;
  await closeChildren();
  await remove(quotes.map((x) => x.id!));
  selection.value = [];
  await load();
}

const bladeToolbar = ref<IBladeToolbar[]>([
  { id: "refresh", title: t("QUOTES.TOOLBAR.REFRESH"), icon: "lucide-refresh-cw", clickHandler: () => load() },
  {
    id: "delete",
    title: t("QUOTES.TOOLBAR.DELETE"),
    icon: "lucide-trash-2",
    permissions: "quote:delete",
    disabled: computed(() => !selection.value.length),
    clickHandler: () => deleteQuotes(selection.value),
  },
]);

// the AngularJS row context menu
const rowActions = (quote: QuoteRequest) => [
  {
    id: "delete",
    icon: "lucide-trash-2",
    title: t("QUOTES.TOOLBAR.DELETE"),
    type: "danger" as const,
    variant: "danger" as const,
    hidden: !hasAccess("quote:delete"),
    clickHandler: () => deleteQuotes([quote]),
  },
];

function openDetails(quote: QuoteRequest) {
  openBlade({ name: "QuoteDetails", param: quote.id });
}

exposeToChildren({ reload: () => load() });
</script>

<style lang="scss">
.quotes-list__number {
  color: var(--color-neutral-900);
}
</style>
