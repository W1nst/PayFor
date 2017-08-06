var moment = require('moment');

module.exports = function (date, format) {
      if (!date) {
        return 'N/A'
      }
      return moment(date, 'YYYY-MM-DD HH:mm:ss').format(format);
};