import { TestBed } from '@angular/core/testing';

import { JwtintercepterService } from './jwtintercepter.service';

describe('JwtintercepterService', () => {
  let service: JwtintercepterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JwtintercepterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
