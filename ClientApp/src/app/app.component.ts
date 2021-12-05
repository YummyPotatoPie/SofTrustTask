import { Component, EventEmitter, Input } from '@angular/core';

// Form main component 
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  public formValues: FormValues = new FormValues();

  public errorPlaceHolders: FormErrorPlaceHolders = new FormErrorPlaceHolders();

  // Проверяет корректность информации и посылает информацию из формы на обработку
  public sendForm(): void {
    this.errorPlaceHolders.clearErrorMessages();

    if (!this.formCorrectness()) {
      this.formValues.selectedClearRequiredValues(this.errorPlaceHolders);
    }
    else {
      let request: XMLHttpRequest = new XMLHttpRequest();
      let requestValues = "Name=" + this.formValues.name + "&Email=" + this.formValues.email + "&Telephone=" + this.formValues.telephone +
        "&Theme=" + (this.formValues.messageTheme.length == 0 ? "Техподдержка" : this.formValues.messageTheme) +
        "&Message=" + (this.formValues.message.length == 0 ? "Пустое сообщение" : this.formValues.messageTheme);

      this.formValues.clearAllValues();
      window.location.replace("/ExecuteQuery?" + requestValues);
    }
  }

  // Проверяет корректность информации в форме
  private formCorrectness(): boolean {
    let correctness: boolean = true;

    if (!FormValidator.validateName(this.formValues.name)) {
      this.errorPlaceHolders.nameErrorMessage = "Имя не должно быть пустым";
      correctness = false;
    }

    if (!FormValidator.validateEmail(this.formValues.email)) {
      this.errorPlaceHolders.emailErrorMessage = "Некорректный адрес электронной почты";
      correctness = false;
    }

    if (!FormValidator.validateTelephone(this.formValues.telephone)) {
      this.errorPlaceHolders.telephoneErrorMessage = "Неккоректный номер телефона";
      correctness = false;
    }

    return correctness;

  }

}

// Класс размещающий информацию об ошибках ввода в места ввода информации
class FormErrorPlaceHolders {

  public emailErrorMessage: string = "";
  
  public telephoneErrorMessage: string = "";

  public nameErrorMessage: string = ""

  public messageThemeErrorMessage: string = ""

  public clearErrorMessages(): void {
    this.emailErrorMessage = "";
    this.nameErrorMessage = "";
    this.telephoneErrorMessage = "";
    this.messageThemeErrorMessage = "";
  }

}

// Класс содержащий методы для валидации данных из формы 
class FormValidator {

  public static validateEmail(email: string): boolean {
    // Почему валидация происходит именно таким образом? 
    // Ответ здесь: https://habr.com/ru/post/175375/
    return new RegExp(".+@.+").test(email);
  }


  public static validateTelephone(telephone: string): boolean {
    // Я пытался найти регулярное выражение для проверки номера телефона в любом формате, но в результате 
    // ни один валидный номер телефона не проходил проверку
    // return new RegExp("/^[+]*[(]{0,1}[0-9]{1,3}[)]{0,1}[-\s\./0-9]*$/g").test(telephone);
    // Поэтому я принял решение написать проверку "втупую"
    let digitsCount: number = 0;

    for (let i: number = 0; i < telephone.length; i++) {
      if (telephone.charAt(i) >= '0' && telephone.charAt(i) <= '9') digitsCount++;
    }

    return digitsCount == 11;
  }

  // Если имя пустое, то возвращает false, иначе true
  public static validateName(name: string): boolean {
    return name.length > 0;
  }

}

// Класс содержащий данные формы
class FormValues {
  
  public name: string = "";

  public email: string = "";

  public telephone: string = "";

  public messageTheme: string = "";

  public message: string = "";

  // Очищает поля name, email и telephone
  // Не совсем удобно если ошибка ввода была только в одном из полей, но ничего умнее я к сожалению не придумал
  // После написания метода  selectedClearRequiredValues не используется
  /*
  public clearRequiredValues(): void {
    this.name = "";
    this.email = "";
    this.telephone = "";
  }
  */

  // Выборочно очищает поля в зависимости от правильности введенного поля
  public selectedClearRequiredValues(formErrorPlaceHolders: FormErrorPlaceHolders): void {
    this.name = formErrorPlaceHolders.nameErrorMessage.length > 0 ? "" : this.name;
    this.email = formErrorPlaceHolders.emailErrorMessage.length > 0 ? "" : this.email;
    this.telephone = formErrorPlaceHolders.telephoneErrorMessage.length > 0 ? "" : this.telephone;
  }

  // Очищает все поля ввода в форме 
  // Используется в случае если форма заполнена корректно и данные отправлены на обработку
  public clearAllValues(): void {
    this.name = "";
    this.email= "";
    this.telephone = "";
    this.messageTheme = "";
    this.message = "";
  }

}
