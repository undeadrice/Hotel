export interface TransactionCodeListResponse {
  id: string;
  transactionGroupId: string;
  transactionGroupName: string;
  code: string;
  name: string;
  defaultAmount: number;
  isActive: boolean;
}
