import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyflightComponent } from './myflight.component';

describe('MyflightComponent', () => {
  let component: MyflightComponent;
  let fixture: ComponentFixture<MyflightComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyflightComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MyflightComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
