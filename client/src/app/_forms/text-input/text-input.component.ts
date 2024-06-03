import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements ControlValueAccessor{
  @Input() label ='';
  @Input() type= 'text';

  //NgControl wiąże fomy z obiektami DOM
  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }
  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }
  // Get po prostu zwaraca od razu wartosc w returnie.
  get control():FormControl{
    return this.ngControl.control as FormControl;
  }
}
