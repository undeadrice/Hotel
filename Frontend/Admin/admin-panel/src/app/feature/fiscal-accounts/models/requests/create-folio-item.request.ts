export interface CreateFolioItemRequest {
  folioId: string;
  description: string;
  quantity: number;
  amount: number;
  transactionCodeId: string;
}
