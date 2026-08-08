export interface TransactionGroupResponse {
  id: string;
  code: string;
  name: string;
  description: string | null;
  type: number;
  isActive: boolean;
  transactionCodes: Array<{
    id: string;
    code: string;
    name: string;
    defaultAmount: number;
    isActive: boolean;
  }>;
}
