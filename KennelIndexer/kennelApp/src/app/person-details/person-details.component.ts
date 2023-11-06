import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PeopleService } from '../services/people.service';
import { Person } from '../models/person';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-person-details',
  templateUrl: './person-details.component.html',
  styleUrls: ['./person-details.component.scss']
})
export class PersonDetailsComponent implements OnInit, OnDestroy {

  private sub: any;
  personId: string;
  person: Person;
  editMode: boolean = false;
  private updateForm;

  images = [];
  effect = 'scrollx';
  test = '102e8b4e-ce49-4aba-80ff-65fc2944ee09.JPG';
  constructor(private route: ActivatedRoute, 
    private peopleService: PeopleService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.personId = params['id'];
   })
   this.updateForm = this.fb.group({
     firstName: [''],
     lastName: [''],
     address: [''],
     reasonsForBeingOnTheList: [''],
   })

   console.log(this.personId)
   if(this.personId != undefined) 
   {
    this.peopleService.getPerson(this.personId).subscribe(res => {
      this.person = {
        personId: res.personId,
        firstName: this.returnEmptyStringIfUndefined(res.firstName), 
        lastName: this.returnEmptyStringIfUndefined(res.lastName),
        address: this.returnEmptyStringIfUndefined(res.address),
        reasonsForBeingOnTheList: this.returnEmptyStringIfUndefined(res.reasonsForBeingOnTheList),
      }
      this.updateForm.patchValue({
        personId: res.personId,
        firstName: res.firstName, 
        lastName: res.lastName,
        address: res.address,
        reasonsForBeingOnTheList: res.reasonsForBeingOnTheList
      })
    });
    this.peopleService.getPictures(this.personId).subscribe(pics => {

      pics.forEach(element => {
        this.images.push(element.pictureUri);
      });
    })
   }
  }

  editPerson(){
    this.editMode = true;
  }

  private returnEmptyStringIfUndefined(value: string): string { 
    value == 'undefined' || value == 'null' ? value = 'N/A' : value = value;
    return value;
  }

  onSubmit(updateForm) {
    const fd = new FormData();
    let firstName = this.returnEmptyStringIfUndefined(updateForm.value.firstName);   
    let lastName = this.returnEmptyStringIfUndefined(updateForm.value.lastName);
    let address = this.returnEmptyStringIfUndefined(updateForm.value.address);
    let reasonsForBeingOnTheList = this.returnEmptyStringIfUndefined(updateForm.value.reasonsForBeingOnTheList);

    fd.append("personId", this.person.personId);
    fd.append("firstName", firstName);
    fd.append("lastName",lastName);
    fd.append("address", address)
    fd.append("reasonsForBeingOnTheList", reasonsForBeingOnTheList);
    
    this.peopleService.putPerson(this.person.personId, fd).subscribe(() => {
      this.person.firstName = firstName;
      this.person.lastName = lastName
      this.person.address = address;
      this.person.reasonsForBeingOnTheList = reasonsForBeingOnTheList;
    });
    console.log("Updated!");
    
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
