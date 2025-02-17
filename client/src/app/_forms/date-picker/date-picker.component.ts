import { Input, Self } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.scss']
})
export class DatePickerComponent implements ControlValueAccessor{
  @Input() label = '';
  @Input() maxDate: Date | undefined;
  @Input() minDate: Date | undefined;
  bsConfig: Partial<BsDatepickerConfig> | undefined;       //Partial - np. 100 rzeczy wymaganych zamieniamy na 100 rzeczy OPCJONALNE.

  constructor(@Self() public ngControl:NgControl) {
    this.ngControl.valueAccessor = this;
    this.bsConfig = {
      containerClass: 'theme-blue',
      dateInputFormat: 'DD MMMM YYYY',

    }
  }
  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }

  get control():FormControl{
    return this.ngControl.control as FormControl;
  }
}
