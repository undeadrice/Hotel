export interface TransactionGroupResponse {
  id: string;
  code: string;
  name: string;
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
