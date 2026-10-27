import { notification, useBlade } from "@vc-shell/framework";
import { toRaw } from "vue";
import { useI18n } from "vue-i18n";
import type { QuoteAddress } from "../api_client/virtocommerce.quote";

// Maps a quote status to a VcStatus variant (dot color of the design's StatusPill).
type Variant = "info" | "warning" | "danger" | "success" | "primary";
const buckets: Record<Variant, string[]> = {
  primary: ["new", "draft"],
  warning: ["processing", "proposal sent"],
  success: ["ordered"],
  danger: ["cancelled", "canceled", "declined", "expired"],
  info: [],
};
export function statusVariant(status?: string): Variant {
  const value = (status ?? "").trim().toLowerCase();
  return (Object.keys(buckets) as Variant[]).find((k) => buckets[k].includes(value)) ?? "info";
}

/** Money formatter (VcDataTable money cells show 0 as "Not set"; quote amounts of 0 are meaningful). */
export function useMoney() {
  const { n } = useI18n({ useScope: "global" });
  return (value?: number | null, currency?: string) =>
    currency
      ? n(value ?? 0, { style: "currency", currency, minimumFractionDigits: 2, maximumFractionDigits: 2 })
      : n(value ?? 0, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

/** Deep copy of (possibly reactive, possibly nested-proxy) entity data. */
export function clone<T>(value: T): T {
  const raw = toRaw(value) as unknown;
  if (raw instanceof Date) return new Date(raw) as T;
  if (Array.isArray(raw)) return raw.map(clone) as T;
  if (raw && typeof raw === "object")
    return Object.fromEntries(Object.entries(raw).map(([k, v]) => [k, clone(v)])) as T;
  return raw as T;
}

/**
 * Line item / configuration images may be stored relative to the platform's asset storage ("catalog/x.jpg");
 * the host page lives under /apps/admin-shell/, so make them absolute.
 * ponytail: assumes the default file-system provider's public path /assets/.
 */
export const assetUrl = (url?: string) => (!url || /^([a-z][a-z0-9+.-]*:|\/)/i.test(url) ? url : `/assets/${url}`);

export const addressName = (a: QuoteAddress) => [a.countryCode, a.regionName, a.city, a.line1].filter(Boolean).join(", ");

/** Opens a product in the Catalog module's blade; that module may not be installed in this host. */
export function useOpenProduct() {
  const { t } = useI18n({ useScope: "global" });
  const { openBlade } = useBlade();
  return (productId?: string) =>
    productId &&
    openBlade({ name: "CatalogProductDetails", param: productId }).catch(() =>
      notification.error(t("QUOTES.ALERTS.PRODUCT_UNAVAILABLE"), { timeout: 5000 }),
    );
}
