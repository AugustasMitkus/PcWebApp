import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import '../index.css';

type Member = {
    id: number;
    firstName: string;
    lastName: string;
    debtSum: number;
}
const Transaction: React.FC = () => {
    const { groupId } = useParams();
    const [formValues] = useState<{ [memberId: number]: number }>({});
    const [members, setMembers] = useState<Member[]>([]);
    const [tempMembers, setTempMembers] = useState<Member[]>([]);
    const [sender, setSender] = useState<number>(0)
    const [transType, setTransType] = useState<number>(1);
    const navigate = useNavigate();
    useEffect (() => {
        const fetchMembers = async () => {
                try {
                    //fetches members of the group alongside their general debt amount (sum)
                    const response = await fetch(`http://localhost:5109/members/group/${groupId}`)
                    if (response.ok) {
                        const data = await response.json();
                        setMembers(data);
                        setTempMembers(data);
                        if (data.length > 0) {
                        setSender(data[0].id);
                        setTempMembers(data.filter((member: { id: any; }) => member.id !== data[0].id));
                        }
                }
                    }
                catch (error:any){
                    console.error("Error:", error.message);
                } 
            };
        fetchMembers();
    },[]);

    const handleSenderChange = async (value: number) => {
        setSender(value);
        try {
            //main purpose is to return all the members except for the one who was chosen
            const response = await fetch(`http://localhost:5109/debts/${value}`);
            if (response.ok) {
                const data = await response.json();
                console.log(data);
                setTempMembers(data);
            }
        } 
        catch (error: any) {
            console.error("Error:", error.message);
        }
    };


    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        try{
            const formData = new FormData(e.currentTarget);
            if (transType !== 0){
                tempMembers.forEach((member) => {
                    const val = formData.get(`value_${member.id}`);
                    formValues[member.id] = Number(val);
                });
            }
            const amount = formData.get("amount");
            //creates a transaction
            const response = await fetch("http://localhost:5109/transactions", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({type: transType, groupId: groupId, senderId: sender, amount: amount, distribution: formValues})
            });
            if (response.ok) {
                const debtUpdateResponse = await fetch(`http://localhost:5109/debts/${sender}`,{
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({type: transType, distribution: formValues, amount: amount})

                });
                if (debtUpdateResponse.ok) {
                    const debtUpdateData = await debtUpdateResponse.json();
                    console.log(debtUpdateData);

                    navigate(`/group/${groupId}`);
                }
                else {
                    const errorData = await debtUpdateResponse.json();
                    console.error("Debt update failed:", errorData);
                    alert("Failed to update debts.");
                    return;
                }
            }
            else {
                const errorData = await response.json();
                console.error("Transaction failed:", errorData);
                alert("Transaction failed.");
                return;
            }

        }
        catch (error: any) {
            console.error("Error submitting transaction:", error.message);
            alert("Something went wrong. Please try again.");
        }
    };

    return(
        <main className="container" >
            <button onClick={() => navigate(`/group/${groupId}`)} className="groupBtn">
                    Back
            </button>
            <h2 className="title">Make a transaction</h2>
            <form className="transaction" onSubmit={handleSubmit}>
            <div className="transaction">
                <label htmlFor="sender">Choose who is paying:</label>
                <select
                    name="sender"
                    id="sender"
                    value={sender}
                    onChange={(e) => handleSenderChange(Number(e.target.value))}
                    style={{ height: "40px", padding: "5px", margin: "20px"}}
                >
                    {members.map((member) => (
                        <option key={member.id} value={member.id}>{member.firstName} {member.lastName}</option>
                    ))}
                </select>
            </div>
            <div className="transaction">
                <label htmlFor="transType">Choose a transaction type:</label>
                <select
                    name="transType"
                    id="transType"
                    value={transType}
                    onChange={(e) => setTransType(Number(e.target.value))}
                    style={{ height: "40px", padding: "5px", margin: "20px"}}
                >
                    <option value={0}>Equal Split</option>
                    <option value={1}>Percentage</option>
                    <option value={2}>Dynamic</option>
                </select>
            </div>
            {(transType === 0 || transType === 1) && (
                <div className="transaction">
                    <label htmlFor="amount">Transaction amount</label>
                    <input required type="number" step={0.01} name="amount" min={0}/>
                </div>
            )}
            {transType === 1 && (
                <div className="trans2">
                    {tempMembers.map((member) => (
                    <div key={member.id}>
                        <label>{member.firstName} {member.lastName} (%)</label>
                        <input
                        type="number"
                        name={`value_${member.id}`}
                        min="0"
                        max="100"
                        required
                        />
                    </div>
                    ))}
                </div>
            )}
            {transType === 2 && (
                <div className="trans2">
                    {tempMembers.map((member) => (
                    <div key={member.id}>
                        <label>{member.firstName} {member.lastName} (Amount)</label>
                        <input
                        type="number"
                        name={`value_${member.id}`}
                        min="0"
                        required
                        />
                        <input type="hidden" name="amount" value="0" />
                    </div>
                    ))}
                </div>
            )}
            <button type="submit" className="groupBtn">Submit</button>
                </form>
        
        </main>
    );
}

export default Transaction;