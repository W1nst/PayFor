var moment = require('moment');
import config from './../config'

module.exports = function (date, format) {
      if (!date) {
        return 'N/A'
      }
      return moment(date, config.dateTimeFormat).format(format);
};