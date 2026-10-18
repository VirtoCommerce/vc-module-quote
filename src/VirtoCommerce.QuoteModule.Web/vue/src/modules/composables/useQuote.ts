import { onUnmounted, type ComputedRef, type Ref } from "vue";
import type { QuoteRequest, ShipmentMethod } from "../../api_client/virtocommerce.quote";

// Like the AngularJS blades, every child blade (items, addresses, attachments, properties) edits the quote
// loaded by QuoteDetails in memory; the quote blade's Save persists everything with one PUT.
// The details blade registers its live state here; children look it up by quote id.
export interface QuoteHandle {
  item: Ref<QuoteRequest>;
  locked: ComputedRef<boolean>;
  shippingMethods: Ref<ShipmentMethod[]>;
  recalculate: () => Promise<void>;
}

const handles = new Map<string, QuoteHandle>();

export function provideQuote(id: string, handle: QuoteHandle) {
  handles.set(id, handle);
  onUnmounted(() => {
    if (handles.get(id) === handle) handles.delete(id);
  });
}

export function useQuote(id?: string): QuoteHandle {
  const handle = id ? handles.get(id) : undefined;
  if (!handle) throw new Error(`[quotes] quote ${id} is not open`);
  return handle;
}

/**
 * The legacy blade keeps item.selectedTierPrice as one of item.proposalPrices (radio buttons work by identity)
 * and lists the tiers by quantity, price.
 */
export function linkSelectedTiers(quote: QuoteRequest) {
  for (const item of quote.items ?? []) {
    item.proposalPrices = (item.proposalPrices ?? []).sort(
      (a, b) => (a.quantity ?? 0) - (b.quantity ?? 0) || (a.price ?? 0) - (b.price ?? 0),
    );
    const s = item.selectedTierPrice;
    item.selectedTierPrice =
      (s && item.proposalPrices.find((p) => p.price === s.price && p.quantity === s.quantity)) || item.proposalPrices[0];
  }
}
