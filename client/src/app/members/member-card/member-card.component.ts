import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.scss'],
  encapsulation: ViewEncapsulation.Emulated
  //  ViewEncapsulation.Emulated - Style tylko dla tego komponentu
  // ViewEncapsulation.Nonne - style globalnie tam, gdzie uzyjemy
  // ViewEncapsulation.ShadowDom- 
})
export class MemberCardComponent implements OnInit{
  @Input() member:Member | undefined;

  constructor() {
  }

  ngOnInit(): void {

  }

}
