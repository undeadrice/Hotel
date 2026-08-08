export interface CreateTransactionGroupRequest {
  code: string;
  name: string;
  description: string | null;
  type: number;
}
