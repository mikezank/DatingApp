import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService)
  cancelRegister = output<boolean>()
  model: any = {}

  register() {
    this.accountService.register(this.model).subscribe({
      next: response => {
        console.log(response)
        this.cancel()
      },
      error: error => console.error(error)
    })
  }

  cancel() {
    this.cancelRegister.emit(false)
  }
}
