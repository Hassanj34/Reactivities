import { useAccount } from '../../lib/hooks/useAccount'
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { loginSchema, type LoginSchema } from '../../lib/schemas/loginSchema';
import { Box, Button, Paper, Typography } from '@mui/material';
import { LockOpen } from '@mui/icons-material';
import TextInput from '../../app/shared/components/TextInput';
import { Link, useLocation, useNavigate } from 'react-router';
import { useState } from 'react';
import { toast } from 'react-toastify';

export default function LoginForm() {
    const [notVerified, setNotVerified] = useState(false);
    const {loginUser, resendConfirmationEmail} = useAccount();
    const navigate = useNavigate();
    const location = useLocation();
    const {control, handleSubmit, watch, formState: { isValid, isSubmitting }} = useForm<LoginSchema>({
        mode: 'onTouched',
        resolver:  zodResolver(loginSchema)
    });

    // eslint-disable-next-line react-hooks/incompatible-library
    const email = watch('email');

    const handleResendEmail = async () => {
        try {
            await resendConfirmationEmail.mutateAsync({email});
            setNotVerified(false);
        } catch (error) {
            console.log(error)
            toast.error("Problem sending email - Please check email address");
        }
    }

    const onSubmit = async (data: LoginSchema) => {
        await loginUser.mutateAsync(data, {
            onSuccess: () => {
                navigate(location.state.from  || '/activties');
            },
            onError: (error) => {
                if(error.message === "NotAllowed"){
                    setNotVerified(true);
                }
            }
        });
    }

  return (
    <Paper 
        component='form' 
        onSubmit={handleSubmit(onSubmit)}
        sx={{display: 'flex', flexDirection: 'column', p: 3, gap: 3, maxWidth: 'md', mx: 'auto', borderRadius: 3}}
    > 
        <Box display='flex' alignItems='center' justifyContent='center' gap={3} color='secondary.main'>
            <LockOpen fontSize='large' />
            <Typography variant='h4'>Sign In</Typography>
        </Box>
        <TextInput label='Email' control={control} name='email' />
        <TextInput label='Password' type='password' control={control} name='password' />
        <Button type='submit' disabled={!isValid || isSubmitting} variant='contained' size='large'>Login</Button>
        {notVerified ? (
            <Box display='flex' flexDirection='column' justifyContent='center'>
                <Typography textAlign='center' color='error'>
                    Your email has not been verified. 
                    You can click the button to re-send the verification email
                </Typography>
                <Button onClick={handleResendEmail} disabled={resendConfirmationEmail.isPending}>Re-send email link</Button>
            </Box>
        ) : (
            <Typography sx={{textAlign:'center'}}>
                Don't have an account?
                <Typography sx={{ml: 1}} component={Link} to='/register' color='primary'>Signup</Typography>
            </Typography>
        )}
    </Paper>
  )
}
