<template>
  <dl class="quotes-totals">
    <template
      v-for="row in rows"
      :key="row.label"
    >
      <dt :class="{ 'quotes-totals__grand': row.grand }">{{ row.label }}</dt>
      <dd :class="{ 'quotes-totals__grand': row.grand }">{{ money(row.value, currency) }}</dd>
    </template>
  </dl>
</template>

<script setup lang="ts">
// the quote's totals as label / amount rows (the quote blade and its line items blade)
import { useMoney } from "../utils";

defineProps<{ rows: { label: string; value?: number; grand?: boolean }[]; currency?: string }>();
const money = useMoney();
</script>

<style lang="scss">
.quotes-totals {
  font-size: 14px;
  display: grid;
  grid-template-columns: 1fr auto;
  gap: 8px 16px;
  margin: 0;
  padding: 16px;
  dd {
    margin: 0;
    text-align: right;
    font-variant-numeric: tabular-nums;
  }
  &__grand {
    border-top: 1px solid var(--color-neutral-200);
    padding-top: 8px;
    font-weight: 700;
    color: var(--color-neutral-950);
  }
}
</style>
