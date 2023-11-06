import { Component, OnInit, ViewChild } from '@angular/core';
import { PeopleService } from 'src/app/services/people.service';
import { Person } from 'src/app/models/person';
import { AddPeopleComponent } from 'src/app/modals/add-people/add-people.component';
import { HttpEventType } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-welcome',
  templateUrl: './name-index.component.html',
  styleUrls: ['./name-index.component.scss']
})
export class NameIndexComponent implements OnInit {


  @ViewChild(AddPeopleComponent, {static: false}) addPerson;

  public searchString: string;
  public isVisible = false;

  listOfData:Person[] = [];
  fullName: string;
  newlyAddedPerson: Person;
  isSpinning = false;
  

  constructor(private peopleService: PeopleService) { }


  showModal(): void {
    this.isVisible = true;
  }

  ngOnInit() : void {
    this.peopleService.getPeople().subscribe
     (
       (response)=>
       {
         response.forEach(element => {
          let firstName = element.firstName.toString();
          let lastName = element.lastName.toString();
          let fullName =  firstName + " " + lastName
          element.firstName = fullName;
          element.lastName = fullName;
          if(element.address == 'undefined') {
            element.address = 'N/A'
          }
          this.listOfData.push(element)
         });   
         console.log(this.listOfData);
             
         this.listOfData = response;
       },
       (error) => console.log(error)
     )    
  }

  handleOk(): void {
    const fd = new FormData();
    if(this.addPerson.firstName != null || undefined && this.addPerson.lastName != null || undefined) 
    {
      fd.append("firstName", this.addPerson.firstName);
      fd.append("lastName", this.addPerson.lastName);
      fd.append("address", this.addPerson.address);
      fd.append("reasonsForBeingOnTheList", this.addPerson.reasonsForBeingOnTheList);

      console.log("FROM name-index with add.person.selectedfile");
      console.log(this.addPerson.selectedFile);
       if(this.addPerson.selectedFile != null || undefined) 
        {
          this.addPerson.selectedFile.forEach(element => {
            Array.from(element).map((file:any, index) => {
              console.log("this is from map");
              console.log(file);
              console.log(element);
             
              fd.append("files"+index, file, file.name);
            })

          });
      
          // fd.append("files", this.addPerson.selectedFile, this.addPerson.selectedFile.name);
          this.addPerson.selectedFile = null;
        }

        console.log("fd right before post");
        console.log(fd)
        this.isSpinning = true;
        this.peopleService.postPeopleWithImage(fd).subscribe(res  => {
          console.log("yep this is res XD")
          console.log(res)
          let a = res as Person;
          console.log(a.firstName)
          
          this.isSpinning = false;
          this.isVisible = false;
          this.addPerson.validateForm.reset();
          this.addPerson.address = '';
          this.addPerson.reasonsForBeingOnTheList = '';
          this.addPerson.reset();

          if(a.address == 'undefined' || null) {
            a.address = 'N/A'
          }
          this.listOfData = [
            ...this.listOfData, 
            {
              personId: a.personId,
              firstName: a.firstName + " " + a.lastName,
              lastName: a.lastName,
              address: a.address,
              reasonsForBeingOnTheList: a.reasonsForBeingOnTheList
            }
          ];
        });
      }
      else 
      {
        for(const i in this.addPerson.validateForm.controls) 
        {
          this.addPerson.validateForm.controls[i].markAsDirty();
          this.addPerson.validateForm.controls[i].updateValueAndValidity();
        }
        console.log("You need at least a firstname and lastname to post something.");
      }
    

  }

  handleCancel(): void {
    console.log('Button cancel clicked!');
    this.isVisible = false;
  }
  deleteRow(id: string): void {
    this.peopleService.deletePerson(id).subscribe(data => {
      this.listOfData = this.listOfData.filter(d => d.personId !== id);
    })
  }

}
