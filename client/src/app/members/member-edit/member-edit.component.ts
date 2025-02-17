import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss'],
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined; //Pobieramy z dziecka formularz o nazwie editForm
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    //Blokuje możliwość przechodzenia na inne karty gdy nie zapisaliusmy edit.
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  member: Member | undefined;
  user: User | null = null;

  constructor(
    private accountServide: AccountService,
    private memberService: MembersService,
    private toastr: ToastrService
  ) {
    this.accountServide.currentUser$.pipe(take(1)).subscribe({
      //Działa tylko jednorazowo. Nie msuimy sie odlaczac od srubscribe
      next: (user) => (this.user = user),
    });
  }

  ngOnInit(): void {
    this.loadMember();
  }
  loadMember() {
    if (!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: (member) => (this.member = member),
    });
  }
  updateMember() {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: (_) => {
        this.toastr.success('Profile updated succcesfully');
        this.editForm?.reset(this.member);
      },
    });
  }
}
