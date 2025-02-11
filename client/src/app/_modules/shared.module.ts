import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';
import {BsDatepickerModule} from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    NgxSpinnerModule.forRoot({
      type: 'ball-circus'
    }),
    FileUploadModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
  ],
  exports:[                    //Trzeba dodac, aby wyexportowac pliki do main modulow.
    ToastrModule,                 //Dodajesz kazdy moduł z imports wyzej.
    NgxSpinnerModule,
    FileUploadModule,
    BsDatepickerModule,
    PaginationModule
  ]
})
export class SharedModule { }
