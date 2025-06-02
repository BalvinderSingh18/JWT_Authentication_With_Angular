import { Component } from '@angular/core';
import { Employee } from '../employee';
import { EmployeeService } from '../employee.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-employee',
  standalone: false,
  templateUrl: './employee.component.html',
  styleUrl: './employee.component.scss'
})
export class EmployeeComponent {
  employeesList:Employee[]=[];
  newEmployee:Employee = new Employee();
  editEmployee:Employee = new Employee();
   constructor(
    private employeeService: EmployeeService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.getAllEmployees();
  }

  getAllEmployees() {
    this.employeeService.getEmployees().subscribe(
      (response) => {
        this.employeesList = response;
      },
      (error) => {
        this.toastr.error('Unable to fetch employee data.', 'Error');
      }
    );
  }

  saveClick() {
    this.employeeService.saveEmployee(this.newEmployee).subscribe(
      () => {
        this.getAllEmployees();
        this.ClearRec();
        this.toastr.success('Employee added successfully!');
      },
      (error) => {
        this.toastr.error('Failed to add employee.', 'Error');
      }
    );
  }

  updateClick() {
    this.employeeService.updateEmployee(this.editEmployee).subscribe(
      () => {
        this.getAllEmployees();
        this.ClearRec();
        this.toastr.success('Employee updated successfully!');
      },
      (error) => {
        this.toastr.error('Update failed.', 'Error');
      }
    );
  }

  deleteClick(id: number) {
    Swal.fire({
      title: 'Are you sure?',
      text: 'This record will be permanently deleted.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'Cancel'
    }).then((result) => {
      if (result.isConfirmed) {
        this.employeeService.deleteEmployee(id).subscribe(
          () => {
            this.getAllEmployees();
            this.toastr.success('Employee deleted successfully!');
          },
          (error) => {
            this.toastr.error('Failed to delete employee.', 'Error');
          }
        );
      }
    });
  }

  editClick(emp: Employee) {
    this.editEmployee = { ...emp };
  }

  ClearRec() {
    this.newEmployee = new Employee();
  }
}
