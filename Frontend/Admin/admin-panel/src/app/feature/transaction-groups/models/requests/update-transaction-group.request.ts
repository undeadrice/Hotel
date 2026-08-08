export interface UpdateTransactionGroupRequest {
  id: string;
  code: string;
  name: string;
  description: string | null;
  type: number;
}
