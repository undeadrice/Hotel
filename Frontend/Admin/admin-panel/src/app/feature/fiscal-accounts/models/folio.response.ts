import { FolioItemResponse } from './folio-item.response';
import { FolioStatus } from '../enums/folio-status.enum';

export interface FolioResponse {
  id: string;
  createdAt: string;
  status: FolioStatus;
  items: FolioItemResponse[];
}
