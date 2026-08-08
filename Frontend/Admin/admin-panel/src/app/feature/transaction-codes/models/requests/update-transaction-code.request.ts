export interface UpdateTransactionCodeRequest {
  id: string;
  transactionGroupId: string;
  code: string;
  name: string;
  description: string | null;
  defaultAmount: number;
}
