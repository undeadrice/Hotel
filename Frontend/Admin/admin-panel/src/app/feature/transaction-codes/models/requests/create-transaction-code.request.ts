export interface CreateTransactionCodeRequest {
  transactionGroupId: string;
  code: string;
  name: string;
  description: string | null;
  defaultAmount: number;
}
