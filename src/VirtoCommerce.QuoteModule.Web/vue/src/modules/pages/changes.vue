<template>
  <VcBlade
    :title="$t('QUOTES.WIDGETS.CHANGES')"
    :loading="loading"
    width="50%"
  >
    <!-- the platform change log (operations widget) of the quote -->
    <VcDataTable
      :items="items"
      state-key="QUOTE_CHANGES"
      :empty-state="{ icon: 'lucide-history', title: $t('QUOTES.PAGES.CHANGES.EMPTY') }"
    >
      <VcColumn
        id="createdDate"
        :title="$t('QUOTES.PAGES.CHANGES.DATE')"
        type="date-ago"
        always-visible
        mobile-role="status"
      />
      <VcColumn
        id="operationType"
        :title="$t('QUOTES.PAGES.CHANGES.TYPE')"
        mobile-role="field"
      />
      <VcColumn
        id="createdBy"
        :title="$t('QUOTES.PAGES.CHANGES.USER')"
        mobile-role="field"
      />
      <VcColumn
        id="detail"
        :title="$t('QUOTES.PAGES.CHANGES.DETAIL')"
        always-visible
        mobile-role="title"
      />
    </VcDataTable>
  </VcBlade>
</template>

<script setup lang="ts">
import { ChangeLogClient, useApiClient, useAsync, useBlade, type OperationLog } from "@vc-shell/framework";
import { VcBlade, VcColumn, VcDataTable } from "@vc-shell/framework/ui";
import { onMounted, ref } from "vue";

defineBlade({ name: "QuoteChanges" });

const { param } = useBlade();
const { getApiClient } = useApiClient(ChangeLogClient);
const items = ref<OperationLog[]>([]);

const { loading, action: load } = useAsync(async () => {
  items.value =
    (await (await getApiClient()).searchChanges({ objectIds: [param.value!], objectType: "QuoteRequest", take: 100 })) ?? [];
});

onMounted(load);
</script>
