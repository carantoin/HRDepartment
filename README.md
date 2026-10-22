# Отдел кадров — ASP.NET Core MVC + GraphQL

Учебный backend на C# / .NET 8. Использует Hot Chocolate, EF Core и SQLite.

## Открытие в Visual Studio Insiders

1. Установите .NET 8 SDK и workload **ASP.NET and web development** (если доступен в вашей редакции Visual Studio).
2. Распакуйте архив.
3. Откройте `HRDepartment.csproj` или папку проекта в Visual Studio Insiders.
4. Дождитесь восстановления NuGet-пакетов.
5. Нажмите `F5` или запустите проект через меню Debug.
6. Откройте адрес приложения и добавьте `/graphql`, например `https://localhost:7000/graphql` (порт будет указан в окне запуска).

При первом запуске автоматически создаётся `hrdepartment.db` и две тестовые записи.

## Если запускаете из терминала

В каталоге проекта:

```bash
dotnet restore
dotnet run
```

Не используйте `dotnet ef` для первого запуска: база создаётся через `EnsureCreated()`. Если в будущем перейдёте на миграции, удалите `EnsureCreated()` и настройте миграции отдельно.

## GraphQL-запросы

### Все сотрудники и личные карточки

```graphql
query {
  employees {
    id
    personnelNumber
    lastName
    firstName
    middleName
    birthDate
    maritalStatus
    phone
    email
    city
    address
    departmentName
    positionTitle
    educations { institution specialty educationLevel graduationYear }
    previousJobs { organization position startDate endDate }
    children { fullName birthDate }
    orders { orderNumber orderDate orderType description }
    absences { type startDate endDate documentNumber }
  }
}
```

### Личная карточка одного сотрудника

```graphql
query {
  employee(id: 1) {
    id
    personnelNumber
    lastName
    firstName
    departmentName
    positionTitle
    educations { institution specialty educationLevel graduationYear }
    previousJobs { organization position startDate endDate }
    children { fullName birthDate }
    orders { orderNumber orderDate orderType description }
    absences { type startDate endDate }
  }
}
```

### Сотрудники без высшего образования

```graphql
query {
  employeesWithoutHigherEducation {
    id
    lastName
    firstName
    positionTitle
    educations { educationLevel institution }
  }
}
```

### Образование не соответствует требуемому для должности

```graphql
query {
  educationMismatch {
    id
    lastName
    firstName
    positionTitle
    requiredEducationLevel
    educations { educationLevel specialty }
  }
}
```

### Приказы сотрудника

```graphql
query {
  ordersForEmployee(employeeId: 1) {
    orderNumber
    orderDate
    orderType
    description
  }
}
```

### Количество детей у каждого сотрудника

```graphql
query {
  childCounts {
    employeeId
    employeeName
    childrenCount
  }
}
```

### Все дети сотрудников

```graphql
query {
  allChildren {
    id
    fullName
    birthDate
    employee { id lastName firstName personnelNumber }
  }
}
```

### Сотрудники, проживающие не в указанном городе

```graphql
query {
  employeesOutsideCity(city: "Москва") {
    lastName
    firstName
    city
    address
  }
}
```

### Сотрудники отдела

```graphql
query {
  employeesByDepartment(departmentName: "ИТ") {
    id
    lastName
    firstName
    positionTitle
    departmentName
  }
}
```

### Отпуска и больничные за период

В запрос попадут записи, которые пересекаются с заданным интервалом.

```graphql
query {
  employeesAbsentDuring(from: "2025-01-01T00:00:00", to: "2025-12-31T23:59:59") {
    type
    startDate
    endDate
    documentNumber
    employee { id lastName firstName departmentName }
  }
}
```

## Мутации

### Добавить сотрудника

```graphql
mutation {
  addEmployee(input: {
    personnelNumber: "HR-003"
    lastName: "Петров"
    firstName: "Пётр"
    middleName: "Ильич"
    birthDate: "1990-01-15T00:00:00"
    gender: "Мужской"
    maritalStatus: "Женат"
    phone: "+7 900 000-00-03"
    email: "petrov@example.com"
    city: "Москва"
    address: "ул. Примерная, 1"
    departmentName: "ИТ"
    positionTitle: "Аналитик"
    requiredEducationLevel: "Высшее"
    hireDate: "2024-09-01T00:00:00"
  }) {
    id
    personnelNumber
    lastName
    firstName
  }
}
```

### Добавить приказ

```graphql
mutation {
  addPersonnelOrder(input: {
    employeeId: 1
    orderNumber: "К-99"
    orderDate: "2025-03-01T00:00:00"
    orderType: "Перевод"
    description: "Перевод в другой отдел"
  }) {
    id
    orderNumber
    orderType
  }
}
```

### Добавить отпуск или больничный

```graphql
mutation {
  addAbsence(input: {
    employeeId: 1
    type: "Больничный"
    startDate: "2025-04-01T00:00:00"
    endDate: "2025-04-05T00:00:00"
    documentNumber: "БЛ-101"
  }) {
    id
    type
    startDate
    endDate
  }
}
```

### Удалить сотрудника

```graphql
mutation {
  deleteEmployee(id: 2)
}
```

## Примечания к логике

- Высшее образование определяется значением `EducationLevel == "Высшее"`. Для учебного проекта это простой единый справочник; в реальной системе значения лучше хранить в enum или отдельной таблице.
- Несоответствие образования определяется сравнением `EducationRecord.EducationLevel` с `Employee.RequiredEducationLevel`. Если для должности предусмотрены несколько допустимых уровней или специальностей, правило нужно расширить.
- Запрос периода отсутствия ищет пересечение интервалов: начало отсутствия не позже конца заданного периода, а конец отсутствия не раньше его начала.
- Это демонстрационный учебный проект. Перед реальным использованием нужно добавить аутентификацию и роли, аудит действий, валидацию и защиту персональных данных.
