export interface TransactionCodeResponse {
  id: string;
  transactionGroupId: string;
  transactionGroupName: string;
  code: string;
  name: string;
  description: string | null;
  defaultAmount: number;
  isActive: boolean;
}
