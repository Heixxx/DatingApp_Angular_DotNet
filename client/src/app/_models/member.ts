import { Photo } from "./photo";

export interface Member{
  pagination: any;
  result: any;
  id:number;
  userName:string;
  photoUrl:string;
  age:string;
  knowAs:string;
  created:Date;
  lastActive:Date;
  gender:string;
  introduction:string;
  lookingFor:string;
  interests:string;
  city:string;
  country:string;
  photos:Photo[];
}
