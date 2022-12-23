﻿using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Models.CustomerAggregate
{
  public interface ICustomerRepository:IRepository<Customer>
  {
  }
}
