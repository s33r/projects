import yargs from 'yargs';
import {hideBin} from 'yargs/helpers';

import * as NewProject from '../../source/Aaron.Automation.NewProject/command.js';


console.log(process.execArgv);

yargs(hideBin(process.argv))
    .command(NewProject)
    .help()
    .argv;