import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    })
  ],
  exports:[                    //Trzeba dodac, aby wyexportowac pliki do main modulow.
    ToastrModule,                 //Dodajesz kazdy moduł z imports wyzej.
  ]
})
export class SharedModule { }
