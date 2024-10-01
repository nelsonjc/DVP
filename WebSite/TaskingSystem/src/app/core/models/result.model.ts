export interface ResultModel<T>{
    success: boolean;
    data: T;
    errorMessage: string | null; 
}

export interface ResultFilterModel<T> {
    data: ResultModel<T>
    metadata: Metadata
  }

export interface Metadata {
    totalCount: number
    pageSize: number
    currentPage: number
    totalPages: number
    hasPreviousPage: boolean
    hasNextPage: boolean
  }
  