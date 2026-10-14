<template>
  <VcBlade
    :title="$t('QUOTES.WIDGETS.ATTACHMENTS')"
    :loading="loading"
    width="50%"
  >
    <VcContainer>
      <div class="quotes-form tw-space-y-4">
        <VcBanner
          v-if="errors.length"
          variant="danger"
          icon="lucide-circle-alert"
        >
          <div
            v-for="(error, i) in errors"
            :key="i"
          >
            {{ error }}
          </div>
        </VcBanner>
        <VcFileUpload
          v-if="canUpload"
          :label="$t('QUOTES.PAGES.ATTACHMENTS.SELECT_FILES')"
          multiple
          :loading="uploading"
          @upload="upload"
        />
        <VcDataTable
          :items="attachments"
          state-key="QUOTE_ATTACHMENTS"
          :row-actions="rowActions"
          row-actions-mode="dropdown"
          row-actions-position="column"
          :empty-state="{ icon: 'lucide-paperclip', title: $t('QUOTES.PAGES.ATTACHMENTS.EMPTY') }"
          @row-click="(e: { data: QuoteAttachment }) => e.data.url && open(e.data.url)"
        >
          <VcColumn
            id="mimeType"
            :title="$t('QUOTES.PAGES.ATTACHMENTS.TYPE')"
            width="80px"
          >
            <template #body="{ data: a }">
              <VcIcon :icon="a.mimeType?.startsWith('image/') ? 'lucide-image' : 'lucide-file-text'" />
            </template>
          </VcColumn>
          <VcColumn
            id="name"
            :title="$t('QUOTES.PAGES.ATTACHMENTS.NAME')"
            always-visible
            mobile-role="title"
          />
        </VcDataTable>
      </div>
    </VcContainer>
  </VcBlade>
</template>

<script setup lang="ts">
// quote-assets: the quote's attachments (FileExperienceApi upload into the quote-attachments scope).
// Uploaded/removed files stay in the quote until it is saved.
import { notification, useApiClient, useAsync, useBlade, usePermissions } from "@vc-shell/framework";
import { VcBanner, VcBlade, VcColumn, VcContainer, VcDataTable, VcFileUpload, VcIcon } from "@vc-shell/framework/ui";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import {
  QuoteModuleClient,
  type FileUploadScopeOptions,
  type QuoteAttachment,
} from "../../api_client/virtocommerce.quote";
import { uploadFiles, type UploadResult } from "../api";
import { useQuote } from "../composables/useQuote";

defineBlade({ name: "QuoteAttachments" });

const { t } = useI18n({ useScope: "global" });
const { options } = useBlade<{ quoteId: string }>();
const { hasAccess } = usePermissions();
const { getApiClient } = useApiClient(QuoteModuleClient);
const quote = useQuote(options.value?.quoteId);

const attachments = computed(() => quote.item.value.attachments ?? []);
const uploadOptions = ref<FileUploadScopeOptions | null>();
const errors = ref<string[]>([]);
const canEdit = computed(() => hasAccess("quote:update") && !quote.locked.value);
const canUpload = computed(() => canEdit.value && !!uploadOptions.value?.scope);
const open = (url: string) => window.open(url, "_blank");

const { loading, action: loadOptions } = useAsync(async () => {
  uploadOptions.value = await (await getApiClient()).getAttachmentOptions().catch(() => null);
  if (canEdit.value && !uploadOptions.value?.scope) errors.value = [t("QUOTES.PAGES.ATTACHMENTS.NOT_CONFIGURED")];
});
onMounted(loadOptions);

const fileError = (fileName: string, code?: string, parameter?: unknown, message?: string) => {
  const known = ["DUPLICATE_NAME", "INVALID_SIZE", "INVALID_EXTENSION"].includes(code ?? "");
  const text = known
    ? t(`QUOTES.PAGES.ATTACHMENTS.${code}`, { parameter: Array.isArray(parameter) ? parameter.join(", ") : parameter })
    : message || t("QUOTES.PAGES.ATTACHMENTS.UNKNOWN_ERROR", { parameter: parameter ?? code ?? "" });
  return t("QUOTES.PAGES.ATTACHMENTS.ERROR", { fileName, message: text });
};

// the legacy client-side checks: unique name, size and extension from the scope options
function validate(file: File): string | undefined {
  const o = uploadOptions.value!;
  if (attachments.value.some((a) => a.name?.toLowerCase() === file.name.toLowerCase()))
    return fileError(file.name, "DUPLICATE_NAME");
  if (o.maxFileSize && file.size > o.maxFileSize) return fileError(file.name, "INVALID_SIZE", o.maxFileSize);
  const allowed = (o.allowedExtensions ?? []).map((x) => x.toLowerCase());
  const ext = /(\.[^.]+)?$/.exec(file.name)?.[1]?.toLowerCase() ?? "";
  if (allowed.length && !allowed.includes(ext)) return fileError(file.name, "INVALID_EXTENSION", allowed);
}

const { loading: uploading, action: upload } = useAsync<FileList>(async (list) => {
  const files = Array.from(list ?? []);
  errors.value = files.map(validate).filter((x): x is string => !!x);
  if (errors.value.length) return;
  const results = await uploadFiles(uploadOptions.value!.scope, files).catch((e): UploadResult[] => [
    { succeeded: false, name: files.map((f) => f.name).join(", "), errorMessage: String(e?.message ?? e) },
  ]);
  for (const result of results) {
    if (result.succeeded === false)
      errors.value.push(fileError(result.name ?? "", result.errorCode, result.errorParameter, result.errorMessage));
    else
      (quote.item.value.attachments ??= []).push({
        name: result.name,
        url: result.url,
        mimeType: result.contentType,
        size: result.size,
      });
  }
});

async function copy(url?: string) {
  if (!url) return;
  const absolute = new URL(url, window.location.origin).href;
  await navigator.clipboard.writeText(absolute);
  notification.success(t("QUOTES.ALERTS.COPIED", { text: absolute }), { timeout: 2500 });
}

const rowActions = (a: QuoteAttachment) => [
  {
    id: "copy",
    icon: "lucide-link",
    title: t("QUOTES.PAGES.ATTACHMENTS.COPY_LINK"),
    type: "info" as const,
    clickHandler: () => copy(a.url),
  },
  {
    id: "remove",
    icon: "lucide-trash-2",
    title: t("QUOTES.PAGES.ATTACHMENTS.REMOVE"),
    type: "danger" as const,
    variant: "danger" as const,
    hidden: !canEdit.value,
    clickHandler: () => (quote.item.value.attachments = attachments.value.filter((x) => x !== a)),
  },
];
</script>
