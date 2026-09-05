import { FolioItemType } from '../enums/folio-item-type.enum';

export interface FolioItemResponse {
  id: string;
  description: string;
  quantity: number;
  amount: number;
  totalAmount: number;
  transactionCodeId: string;
  transactionGroupType: FolioItemType;
  businessDate: string;
  createdAt: string;
}
