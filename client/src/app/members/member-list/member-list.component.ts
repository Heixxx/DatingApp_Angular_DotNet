import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.scss'],
})
export class MemberListComponent implements OnInit {
  // members$: Observable<Member[]> | undefined;                            // $ - obserwowalny
  members: Member[] = [];
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 5;

  constructor(private memberSrvices: MembersService) {}

  ngOnInit(): void {
    // this.members$ = this.memberSrvices.getMembers();
    this.loadMembers();
  }

  loadMembers() {
    this.memberSrvices.getMembers(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        if (response.result && response.pagination) {
          this.members = response.result;
          this.pagination = response.pagination;
        }
      },
    });
  }
  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMembers();
    }
  }
}
