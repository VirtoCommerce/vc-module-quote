<template>
  <VcPopup
    :title="$t('QUOTES.CANCEL.TITLE')"
    variant="warning"
    @close="$emit('close')"
  >
    <template #content>
      <div class="quotes-cancel">
        <p>{{ $t("QUOTES.CANCEL.MESSAGE") }}</p>
        <VcTextarea
          v-model="reason"
          :label="$t('QUOTES.CANCEL.REASON')"
          :placeholder="$t('QUOTES.CANCEL.REASON_PLACEHOLDER')"
          required
        />
      </div>
    </template>
    <template #footer="{ close }">
      <VcButton
        variant="secondary"
        @click="close()"
      >
        {{ $t("QUOTES.CANCEL.BACK") }}
      </VcButton>
      <VcButton
        :disabled="!reason.trim()"
        @click="$emit('confirm', reason)"
        >{{ $t("QUOTES.CANCEL.CONFIRM") }}</VcButton
      >
    </template>
  </VcPopup>
</template>

<script setup lang="ts">
// cancelQuote-dialog.tpl.html: the reason is required, confirm/cancel
import { VcButton, VcPopup, VcTextarea } from "@vc-shell/framework/ui";
import { ref } from "vue";

defineEmits<{ (e: "close"): void; (e: "confirm", reason: string): void }>();
const reason = ref("");
</script>

<style scoped>
.quotes-cancel {
  display: flex;
  flex-direction: column;
  gap: 12px;
}
</style>
