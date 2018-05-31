pragma solidity ^0.4.11;
contract Status {
  uint public timestamp;
  uint public state;
 constructor()
 {
    state = 0;
    timestamp = now;
 }
 function funded() public
 {
   state = 1;
   timestamp = now;

 }
 function refunded() public
 {
   state = 2;
   timestamp = now;

 }
 function expired() public
 {
   state = 3;
   timestamp = now;

 }
 function released() public
 {
   state = 4;
   timestamp = now;
 }
}
contract Escrow {
    uint balance;
    uint balanceEscrow;
    address public buyer;
    address public seller;
    address public escrow;
    uint private start;
    uint public amount;
    Status escrow_state;
    bool buyerOk;
    bool sellerOk;
    constructor(address buyer_address, address escrow_address, uint escrow_value) public {
        // this is the constructor function that runs ONCE upon initialization
        buyer = buyer_address;
        seller = msg.sender;
        escrow = escrow_address;
        amount = escrow_value;
        start = now; //now is an alias for block.timestamp, not really "now"
        escrow_state = new Status(); //state of escrow 0 created, 1 funded, 2 refunded, 3 expired, 4 released

    }
    function GetStatus() public returns (Status){
      return escrow_state;

    }
    function accept() public {
        if (msg.sender == buyer){
            buyerOk = true;
        } else if (msg.sender == seller){
            sellerOk = true;
        }
        if (buyerOk && sellerOk){
            payBalance();
        } else if (buyerOk && !sellerOk && now > start + 30 days) {
            // Freeze 30 days before release to buyer. The customer has to remember to call this method after freeze period.
            selfdestruct(buyer);
        }
    }

    function payBalance() private {
        // we are sending ourselves (contract creator) a fee
        escrow.transfer(this.balance / 100);
        // send seller the balance
        if (seller.send(this.balance)) {
            balance = 0;
            escrow_state.released();
        } else {
            throw;
        }
    }
    function depositEscrow() public payable {
      if(msg.sender == seller){
        balance += msg.value;
      }

    }
    function deposit() public payable {
        if (msg.sender == buyer) {
            balance += msg.value;
            escrow_state.funded();
        }
    }

    function cancel() public {
        if (msg.sender == buyer){
            buyerOk = false;
        } else if (msg.sender == seller){
            sellerOk = false;
        }
        // if both buyer and seller would like to cancel, money is returned to buyer
        if (!buyerOk && !sellerOk){
            selfdestruct(buyer);
        }
    }

    function kill() public constant {
        if (msg.sender == escrow) {
            selfdestruct(buyer);

        }
    }
}
