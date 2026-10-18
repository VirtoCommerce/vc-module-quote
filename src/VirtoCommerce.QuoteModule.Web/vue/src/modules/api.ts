// Small same-origin client for the sibling-module endpoints the Quote blades read
// (members, stores, products, prices, countries, settings, file upload).
// ponytail: hand-written instead of generating five more module clients; generate them if these grow.

export interface SearchResult<T> {
  results?: T[];
  totalCount?: number;
}

export interface NamedEntity {
  id?: string;
  name?: string;
  memberType?: string;
}

export interface ProductEntry {
  id?: string;
  code?: string;
  name?: string;
  imageUrl?: string;
}

export interface Product {
  id?: string;
  code?: string;
  name?: string;
  catalogId?: string;
  categoryId?: string;
  imgSrc?: string;
  taxType?: string;
}

export interface Price {
  currency?: string;
  list?: number;
  sale?: number;
}

export interface UploadResult {
  succeeded?: boolean;
  errorCode?: string;
  errorParameter?: string;
  errorMessage?: string;
  id?: string;
  name?: string;
  url?: string;
  contentType?: string;
  size?: number;
}

async function request<T>(method: string, url: string, body?: unknown): Promise<T> {
  const response = await fetch(url, {
    method,
    credentials: "same-origin",
    headers: body === undefined ? { Accept: "application/json" } : { "Content-Type": "application/json", Accept: "application/json" },
    body: body === undefined ? undefined : JSON.stringify(body),
  });
  const text = await response.text();
  if (!response.ok) throw new Error(text || `${response.status} ${response.statusText}`);
  return (text ? JSON.parse(text) : undefined) as T;
}

/** VcSelect async options: (keyword, skip, ids) → { results, totalCount } */
export const searchEmployees = (keyword?: string, skip?: number, ids?: string[]) =>
  request<SearchResult<NamedEntity>>("POST", "/api/members/search", {
    memberType: "Employee",
    deepSearch: true,
    sort: "name",
    keyword: ids?.length ? undefined : keyword,
    objectIds: ids?.length ? ids : undefined,
    skip: skip ?? 0,
    take: 20,
  });

export const getMemberByUserId = (userId: string) =>
  request<NamedEntity>("GET", `/api/members/accounts/${encodeURIComponent(userId)}`);

/** VcSelect async options: the quote's store resolves by id, the rest pages on demand */
export const searchStores = (keyword?: string, skip?: number, ids?: string[]) =>
  request<SearchResult<NamedEntity>>("POST", "/api/stores/search", {
    keyword: ids?.length ? undefined : keyword,
    objectIds: ids?.length ? ids : undefined,
    skip: skip ?? 0,
    take: ids?.length || 20,
  });

// settings/values/{name} needs only a signed-in user (legacy settings.getValues)
export const settingValues = (name: string) =>
  request<unknown[] | null>("GET", `/api/platform/settings/values/${encodeURIComponent(name)}`).then((x) =>
    (x ?? []).map(String),
  );

export async function searchProducts(keyword: string | undefined, skip: number, take: number) {
  const r = await request<{ listEntries?: ProductEntry[]; totalCount?: number }>("POST", "/api/catalog/listentries", {
    keyword,
    objectTypes: ["CatalogProduct"],
    searchInChildren: true,
    searchInVariations: true,
    skip,
    take,
  });
  return { results: r.listEntries ?? [], totalCount: r.totalCount ?? 0 };
}

export const getProduct = (id: string) => request<Product>("GET", `/api/catalog/products/${encodeURIComponent(id)}`);
export const getProductPrices = (id: string) =>
  request<Price[]>("GET", `/api/products/${encodeURIComponent(id)}/prices`).then((x) => x ?? []);

let countries: Promise<{ id?: string; name?: string }[]> | undefined;
export const getCountries = () => (countries ??= request<{ id?: string; name?: string }[]>("GET", "/api/platform/common/countries"));
export const getRegions = (countryCode: string) =>
  request<{ id?: string; name?: string }[]>("GET", `/api/platform/common/countries/${encodeURIComponent(countryCode)}/regions`);

/** FileExperienceApi upload (multipart): one result per file */
export async function uploadFiles(scope: string, files: File[]): Promise<UploadResult[]> {
  const body = new FormData();
  files.forEach((f) => body.append("file", f, f.name));
  const response = await fetch(`/api/files/${encodeURIComponent(scope)}`, {
    method: "POST",
    credentials: "same-origin",
    headers: { Accept: "application/json" },
    body,
  });
  const text = await response.text();
  if (!response.ok) throw new Error(text || `${response.status} ${response.statusText}`);
  return text ? JSON.parse(text) : [];
}
