
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using Lab3OOp.Domain.DTOs;

namespace Lab3OOp.Domain.Validators
{
    public class StudentValidator : AbstractValidator<StudentDTO>, IValidator<StudentDTO>
    {
        public StudentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters");
            RuleFor(x => x.Grade).InclusiveBetween(0, 100).WithMessage("Grade must be between 0 and 100");
        }
    }
}
