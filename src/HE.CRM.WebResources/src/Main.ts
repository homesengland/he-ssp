import { Account } from './events/Account'
import { Isp } from './events/Isp'
import { VfT } from './events/VfT'
import { Precomplete } from './events/Precomplete'
import { Condition } from './events/Condition'
import { LoanApplication } from './events/LoanApplication'

export function initialize() {
  console.log('Initialize main library')
}

exports.Account = Account
exports.Isp = Isp
exports.VfT = VfT
exports.Precomplete = Precomplete
exports.Condition = Condition
exports.LoanApplication = LoanApplication
