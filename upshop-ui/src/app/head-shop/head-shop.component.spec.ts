import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HeadShopComponent } from './head-shop.component';

describe('HeadShopComponent', () => {
  let component: HeadShopComponent;
  let fixture: ComponentFixture<HeadShopComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HeadShopComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeadShopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
