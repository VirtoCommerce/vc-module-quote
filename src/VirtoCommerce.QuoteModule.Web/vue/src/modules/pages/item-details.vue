<template>
  <VcBlade
    :title="line?.name ?? ''"
    width="40%"
  >
    <VcContainer>
      <VcForm class="quotes-form tw-space-y-4">
        <div class="quotes-link-field">
          <VcLabel>{{ $t("QUOTES.PAGES.ITEM.PRODUCT") }}</VcLabel>
          <VcLink @click="openProduct(line?.productId)">{{ line?.name }}</VcLink>
        </div>
        <VcField
          v-if="line?.comment"
          :label="$t('QUOTES.PAGES.ITEM.COMMENT')"
          :model-value="line.comment"
        />
      </VcForm>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
// quote-item-detail: the product link, its comment and (for configurable products) the configuration widget
import { useBlade, useBladeWidgets } from "@vc-shell/framework";
import { VcBlade, VcContainer, VcField, VcForm, VcLabel, VcLink } from "@vc-shell/framework/ui";
import { computed } from "vue";
import { useQuote } from "../composables/useQuote";
import { useOpenProduct } from "../utils";

defineBlade({ name: "QuoteItemDetails" });

const { options, openBlade } = useBlade<{ quoteId: string; index: number }>();
const openProduct = useOpenProduct();
const quote = useQuote(options.value?.quoteId);
const line = computed(() => quote.item.value.items?.[options.value?.index ?? -1]);

useBladeWidgets([
  {
    id: "QuoteItemConfiguration",
    icon: "lucide-sliders-horizontal",
    title: "QUOTES.WIDGETS.CONFIGURATION",
    isVisible: computed(() => !!line.value?.configurationItems?.length),
    onClick: () => openBlade({ name: "QuoteItemConfiguration", options: options.value }),
  },
]);
</script>
