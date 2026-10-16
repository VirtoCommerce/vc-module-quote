<template>
  <VcBlade
    :title="$t('QUOTES.WIDGETS.CONFIGURATION')"
    width="50%"
  >
    <VcContainer>
      <div class="quotes-form tw-space-y-4">
        <VcCard :header="$t('QUOTES.PAGES.CONFIGURATION.PRODUCTS')">
          <VcDataTable
            :items="byType('Product')"
            state-key="QUOTE_CONFIG_PRODUCTS"
            :empty-state="{ icon: 'lucide-package', title: $t('QUOTES.PAGES.CONFIGURATION.NO_PRODUCTS') }"
            @row-click="(e: { data: QuoteConfigurationItem }) => openProduct(e.data.productId)"
          >
            <VcColumn
              id="imageUrl"
              :title="$t('QUOTES.PAGES.CONFIGURATION.IMAGE')"
              width="80px"
              mobile-role="image"
            >
              <template #body="{ data: row }">
                <VcImage
                  :src="assetUrl(row.imageUrl)"
                  size="s"
                  aspect="1x1"
                  bordered
                  background="contain"
                />
              </template>
            </VcColumn>
            <VcColumn
              id="name"
              :title="$t('QUOTES.PAGES.CONFIGURATION.NAME')"
              mobile-role="title"
            />
            <VcColumn
              id="quantity"
              :title="$t('QUOTES.PAGES.CONFIGURATION.QUANTITY')"
              type="number"
              mobile-role="field"
            />
          </VcDataTable>
        </VcCard>
        <VcCard :header="$t('QUOTES.PAGES.CONFIGURATION.TEXTS')">
          <VcDataTable
            :items="byType('Text')"
            state-key="QUOTE_CONFIG_TEXTS"
            :empty-state="{ icon: 'lucide-type', title: $t('QUOTES.PAGES.CONFIGURATION.NO_TEXTS') }"
          >
            <VcColumn
              id="customText"
              :title="$t('QUOTES.PAGES.CONFIGURATION.TEXT')"
              mobile-role="title"
            />
            <VcColumn
              id="quantity"
              :title="$t('QUOTES.PAGES.CONFIGURATION.QUANTITY')"
              type="number"
              mobile-role="field"
            />
          </VcDataTable>
        </VcCard>
        <VcCard :header="$t('QUOTES.PAGES.CONFIGURATION.FILES')">
          <VcDataTable
            :items="files"
            state-key="QUOTE_CONFIG_FILES"
            :empty-state="{ icon: 'lucide-file', title: $t('QUOTES.PAGES.CONFIGURATION.NO_FILES') }"
            @row-click="(e: { data: QuoteConfigurationItemFile }) => e.data.url && open(e.data.url, '_blank')"
          >
            <VcColumn
              id="name"
              :title="$t('QUOTES.PAGES.CONFIGURATION.FILE_NAME')"
              mobile-role="title"
            />
            <VcColumn
              id="contentType"
              :title="$t('QUOTES.PAGES.CONFIGURATION.CONTENT_TYPE')"
              mobile-role="field"
            />
            <VcColumn
              id="size"
              :title="$t('QUOTES.PAGES.CONFIGURATION.SIZE')"
              type="number"
              mobile-role="field"
            />
          </VcDataTable>
        </VcCard>
      </div>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
// quote-item-configuration (+ -products / -texts / -files): the configured product's sections in one blade
import { useBlade } from "@vc-shell/framework";
import { VcBlade, VcCard, VcColumn, VcContainer, VcDataTable, VcImage } from "@vc-shell/framework/ui";
import { computed } from "vue";
import type { QuoteConfigurationItem, QuoteConfigurationItemFile } from "../../api_client/virtocommerce.quote";
import { useQuote } from "../composables/useQuote";
import { assetUrl, useOpenProduct } from "../utils";

defineBlade({ name: "QuoteItemConfiguration" });

const { options } = useBlade<{ quoteId: string; index: number }>();
const openProduct = useOpenProduct();
const quote = useQuote(options.value?.quoteId);
const items = computed(() => quote.item.value.items?.[options.value?.index ?? -1]?.configurationItems ?? []);

const byType = (type: string) => items.value.filter((x) => x.type === type);
const files = computed(() => byType("File").flatMap((x) => x.files ?? []));
const open = (url: string, target: string) => window.open(url, target);
</script>
