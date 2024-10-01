import { TestBed } from '@angular/core/testing';

import { ApiResponseHandlerService } from './api-response-handler.service';

describe('ApiResponseHandlerService', () => {
  let service: ApiResponseHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApiResponseHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
