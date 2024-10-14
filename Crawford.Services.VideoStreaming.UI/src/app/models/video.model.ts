import { Guid } from "guid-typescript";

export interface Video {
    id: Guid;
    title: string;
    description: string;
    categories: string;
    filePath: string;
    thumbnailPath: string;
    uploadedAt: Date;
  }