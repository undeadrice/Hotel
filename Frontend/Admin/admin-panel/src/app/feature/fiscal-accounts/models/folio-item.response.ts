import { TransactionType } from '../../transaction-groups/enums/transaction-type.enum';

export interface FolioItemResponse {
  id: string;
  description: string;
  quantity: number;
  amount: number;
  transactionCodeId: string;
  transactionGroupType: TransactionType;
  createdAt: string;
}
